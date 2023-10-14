using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Playables;

public enum CharacterType
{
    None,
    William,
    Kinki,
    Cheif,
    Jack,
    Vampire
}

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        private float height;
        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();

        CharacterType npcType = CharacterType.None;
        public CharacterType NPCType { get { return npcType; } set { npcType = value; } }

        public static string[] likeGrades = { "처음 만난 사이", "마을 주민", "잘 아는 사람", "친구", "절친" };
        [SerializeField] int likeGrade = 0;
        [SerializeField] int like = 0;
        public int Like { get { return like; } }
        void AddLike(int num) 
        { 
            like += num;
            if (like == 0)
                likeGrade = 0;
            if (like < 20)
                likeGrade = 1;
            if (like >= 20)
                likeGrade = 2;
            if (like >= 70)
                likeGrade = 3;
            if (like >= 100)
                likeGrade = 4;
        }

        public string nowState = "대화중";

        public static string player_name = "example";

        string prompt_prev = "아래의 지시사항에 맞게 대화해줘. "
            + " \n 너는 상대방 (이름 :"+ player_name +") 과 대화하고 있어. 아래에 너의 정보가 있으니 컨셉에 맞게 대화해줘. 호감도가 높을수록 친밀하게, 낮을수록 딱딱하게 말해줘.";

        private string prompt_william =
           "\n\n<<너(chatGPT)의 정보>>\n{농사일 외에는 관심이 없는 무뚝뚝한 23세 농부 윌리엄. 요즘에는 옥수수농사를 하고 있다. 최대한 무뚝뚝한 말투로 짧게 말한다. 자기 얘기는 잘 안한다.} ";

        private string prompt_kinki =
            "\n\n<<너의 정보>>\n{ 햄버거를 제일 좋아하는 먹방유튜버 킨키. 나이는 17세이며 귀여운 말투로 말한다. 저녁 6시마다 방송을 한다.";

        private string prompt_cheif =
             "\n\n<<너의 정보>>\n{마을의 촌장인 67세 중년 남성 로버트. 호탕한 말투로, '하하하' 하고 웃는 것이 말버릇이다. 마을에 대해 잘 알고 있으며 마을과 농사에 대해 알려주고 싶어하지만, 친절하지는 않다.";

        string prompt_common_info = "\n\n<<마을에 대한 정보>> \r\n" +
            "1. 마을 주민 : 킨키, 로버트, 뱀파이어(이름 불명), 윌리엄, 잭, " + player_name + "(대화상대)가 있다. " +
            "\r\n2. 킨키는 먹방 유튜버로 햄버거를 좋아하는 여자이다. \r\n로버트은 촌장이다. '뱀파이어'는 자신이 뱀파이어라고 주장하는 미스터리의 여자이다." +
            "\r\n윌리엄은 마을의 농부로, 일하는 것을 좋아하는 남자다.\r\n잭은 마을의 유일한 식당의 요리사로, 남자이다. " +
            "\r\n"+player_name + "(대화상대)는 마을에 온지 오래되지 않았고 농사를 배우고 있는 청년이다. " +
            "\r\n3. 대화상대인" +player_name + "는 달리거나 농사를 짓거나, 나무를 흔드는 등 행동을 하면 에너지가 소모된다.\r\n에너지를 충전하기 위해서는 요리를 하거나 식당에서 사서 음식을 먹어야 한다. \r\n";

        string prompt_common_last = "\n\n1~2문장으로만 말해줘!짧게 20개 이내의 단어로 말해줘." + "위의 정보는 참고만 하고 설명하지는 마. 자연스럽게 대화만 해 줘. 그리고 호감도가 높을수록 더욱 친밀한 말투로 말해줘. chatGPT같지 않게, 너의 특징을 잘 살려서 진짜 캐릭터처럼 해줘\n\n\n";

        private void Start()
        {
            button.onClick.AddListener(SendReply);

            
        }

        void Update()
        {
            // Enter키 동작
            if (Input.GetKeyDown(KeyCode.Return) && inputField.GetComponent<InputField>().text.Length > 0)
            {
                SendReply();
            }
        }

        public void ResetDialogs()
        {
            Transform contentObject = scroll.gameObject.transform.GetChild(0).GetChild(0);
            int size = contentObject.childCount;
            for (int i = 0; i < size; i++)
            {
                Destroy(contentObject.GetChild(i).gameObject);
            }
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
            
        }

        private async void SendReply()
        {
            if (messages.Count > 4)
                messages.RemoveAt(0);

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);
            newMessage.Content = prompt_prev;

            if (messages.Count == 0)
            {
                switch(npcType)
                {
                    case CharacterType.None:
                        break;
                    case CharacterType.William:
                        newMessage.Content += prompt_william;
                        break;
                    case CharacterType.Kinki:
                        newMessage.Content = prompt_kinki;
                        break;
                    case CharacterType.Cheif:
                        newMessage.Content += prompt_cheif;
                        break;
                }
            }

            newMessage.Content += "\n상대방과의 관계:" + likeGrades[likeGrade] + "\t 현재 너가 하고있던 것 : " +nowState + prompt_common_info + "\n" + prompt_common_last + inputField.text;

            messages.Add(newMessage);
            
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            button.enabled = true;
            inputField.enabled = true;
        }
    }
}
