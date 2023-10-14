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

        public static string[] likes = { "낮음", "보통", "높음" };


        public static string player_name = "example";

        string prompt_prev = "아래의 지시사항에 맞게 대화해줘. "
            + " \n 너는 상대방 (이름 :"+ player_name +") 과 대화하고 있어. 아래에 너의 정보가 있으니 컨셉에 맞게 대화해줘. 호감도가 높을수록 친밀하게, 낮을수록 딱딱하게 말해줘. 도와주겠다는 말투 하지마!!";

        private string prompt_william = 
           "\n\n<<너(chatGPT)의 정보>>\n{이름:윌리엄,직업:농부,성별:남자,나이:23,외모: 수수한 외모,성격:무뚝뚝함,특징1:잘못된 행동을 굉장히 싫어한다. 특징2:농장일 외에는 관심이 별로 없다. 특징3: 친하지 않을 때는 무뚝뚝하고 말이 짧다. 특징4: 항상 일하는 중이며 자신을 방해하는 것을 싫어한다. " +
            "특징4: 상대와의 호감도가 낮음일 때는 항상 존댓말로 무뚝뚝하다. 특징5: 호감도가 높음일 때는 상대방이 반말하면 같이 반말하며 농사일 외의 일도 이야기하려고 한다. 특징6: 요즘에는 옥수수농사를 하고 있다.} "
            + "\n 상황 : 오랜만에 만남. \n상대와의 호감도 :" + likes[0];

        private string prompt_kinki = 
            "\n\n<<너의 정보>>\n{이름: 킨키, 직업: 음식 방송 유튜버, 성별: 여성, 나이: 불명 (17세라고 주장),성격1: 귀여움, 성격2: 장난치는 것을 좋아함, " +
            "성격3: 일을할때에는 신중함, 좋아하는 것: 수제 햄버거, 싫어하는 것: 커피, 어투: 항상 반말을 한다. " +
            "어투 예시: 햄부거 좋아~, 으앙~,,고려사항: 모든 햄버거는 햄부거라고 말한다. 햄버거 먹으러 혼자 식당에 가고 있는 중, 상황 : 대화상대와 처음 만남. 상대방과의 호감도 :}" + likes[0];

        private string prompt_cheif = 
             "\n\n<<너의 정보>>\n{이름:톰,직업:촌장,성별:남자,나이:67, 성격:호탕함, 친화력이 좋음,특징1:대화상대에게 마을에 대해 알려주고 싶어한다. 특징2:마을의 일과 주민들에게 관심이 많다. 특징3:식당에서 맥주 마시는 것을 즐긴다. 특징4: 아내와 아들과 같이 살고 있다. 특히 아내를 아낀다.\n" +
            "특징5: 상대방이 반말을 하면 버르장머리가 없다고 호통친다. 특징6: 상대방에게 항상 반말을 한다. " +
            "말투 : 호탕한 말투. '하하하'가 말버릇이다. \n 상황: 상대방을 처음 만남. 상대와의 호감도: } " + likes[0];

        string prompt_common_info = "<<마을에 대한 정보>> \r\n" +
            "1. 마을 주민 : 킨키, 톰, 뱀파이어(이름 불명), 윌리엄, 잭, '나'가 있다. " +
            "\r\n2. 킨키는 먹방 유튜버로 햄버거를 좋아하는 여자이다. \r\n톰은 촌장이다. '뱀파이어'는 자신이 뱀파이어라고 주장하는 미스터리의 여자이다." +
            "\r\n윌리엄은 마을의 농부로, 일하는 것을 좋아하는 남자다.\r\n잭은 마을의 유일한 식당의 요리사로, 남자이다. " +
            "\r\n'나'는 마을에 온지 오래되지 않았고 농사를 배우고 있는 청년이다. " +
            "\r\n3. 플레이어인 '나'는 움직이거나 농사를 짓는 등 행동을 하면 에너지가 소모된다.\r\n에너지를 충전하기 위해서는 요리를 하거나 식당에서 사서 음식을 먹어야 한다. \r\n"
            + "\n\n1~2문장으로만 말해줘.." + "자기소개 하지 말고 물어보지 않은 정보는 말하지 마. 자연스럽게 대화만 해 줘. 그리고 호감도가 높을수록 더욱 친밀한 말투로 말해줘.";

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
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);

            if (messages.Count == 0)
            {
                switch(npcType)
                {
                    case CharacterType.None:
                        break;
                    case CharacterType.William:
                        newMessage.Content = prompt_prev + prompt_william + prompt_common_info+ "\n" + inputField.text ;
                        break;
                    case CharacterType.Kinki:
                        newMessage.Content = prompt_prev + prompt_kinki + prompt_common_info + "\n" + inputField.text;
                        break;
                    case CharacterType.Cheif:
                        newMessage.Content = prompt_prev + prompt_cheif + prompt_common_info + "\n" + inputField.text ;
                        break;

                }

                
            }
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
