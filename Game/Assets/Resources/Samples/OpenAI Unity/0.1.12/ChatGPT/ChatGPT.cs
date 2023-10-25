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

        public static string[] likeGrades = { "first meeting(unfamiliar,feistiness)", "one met, but unfamiliar, feistiness ", "familiar,Kindness", "friend,tenderness", "best friend, love" };
        int likeGrade;

        public string nowState = "talking";

        public static string player_name = "Jiwon";

        //Gift
        public static string gift_name;

        public static string[] giftGrades = { "You really hate this.", "You think it's not bad.", "You really love this." };

        public int giftGrade;

        //Prompt
        string prompt_prev = " \n you are the below character, and talking with (이름 :" + player_name + ") . don't use  \"\" or : ";

        private string prompt_william =
           "\n\n<<너가 연기할 인물의 정보>>{농사일 외에는 관심이 없는 무뚝뚝한 23세 농부 윌리엄. 요즘에는 옥수수농사를 하고 있다. 최대한 무뚝뚝한 말투로 짧게 말한다. 자기 얘기는 잘 안한다.} ";

        private string prompt_kinki =
            "\n\n<<you are>>{ Kinki. 20 years old. speak in a cute way. a internet streamer(mukbang). have no interest in others. so never help others. and little stupid. love hamburger. live alone.";


        private string prompt_cheif =
            "\n\n<<you are >>{Robert. 67 years old, a cheif of the town. usually laugh like 'haha'. know well about town. like beer so drinks every day. }"
           + "favorite gifts are steak and egg. worse gifts are cookie, cake and tomato";

        private string prompt_vampire =
            "\n\n<<너가 연기할 인물의 정보>> {이름과 나이가 비밀인 젊은 여성. 자신을 뱀파이어라고 생각한다. 저녁부터 새벽 사이에만 마을을 돌아다닌다. 고상한 말투.'호호'를 붙여 말한다.";

        string prompt_common_info = "\n\n<<Information about this town>> \r\n" +
            "1. 마을 주민 : 킨키, 로버트, 뱀파이어(이름 불명), 윌리엄, 잭, " + player_name + "(대화상대)가 있다. " +
            "\r\n2. 킨키는 먹방 유튜버로 햄버거를 좋아하는 여자이다. \r\n로버트은 촌장이다. '뱀파이어'는 자신이 뱀파이어라고 주장하는 미스터리의 여자이다." +
            "\r\n윌리엄은 마을의 농부로, 일하는 것을 좋아하는 남자다.\r\n잭은 마을의 유일한 식당의 요리사로, 남자이다. " +
            "\r\n너의 대화상대인 ("+player_name + ")는 마을에 온지 오래되지 않았고 농사를 하는 청년이다. " +
            "\r\n3. 너의 대화상대인" +player_name + "는 달리거나 농사를 짓거나, 나무를 흔드는 등 행동을 하면 에너지가 소모된다.\r\n에너지를 충전하기 위해서는 요리를 하거나 식당에서 사서 음식을 먹어야 한다. \r\n";

        string prompt_common_last = "\n\nSay only 1~2 Korean sentence. (use within 20 words) " + "Don't explain upper info. Talk like real character, not chatGPT. Don't say you will help me.\n";

        string prompt_common_gift;

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

        public void UpdateGiftPrompt(string giftId, int grade)
        {
            gift_name = Managers.Data.GetItemData(giftId).name;
            giftGrade = grade;

            prompt_common_gift = "\nAnd now you're getting present from me. It is " + gift_name + ", and " + giftGrades[giftGrade] + "Respond to it.\n\n\n";
        }

        public void UpdateLikeability()
        {
            likeGrade = gameObject.GetComponent<Likeability>().Grade;
        }

        public void ResetDialogs()
        {
            Transform contentObject = scroll.gameObject.transform.GetChild(0).GetChild(0);
            int size = contentObject.childCount;
            for (int i = 0; i < size; i++)
            {
                Destroy(contentObject.GetChild(i).gameObject);
                messages.Clear();
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
            gameObject.GetComponent<Likeability>().Increase(0.2f);
            if (messages.Count > 4)
            {
                messages.RemoveAt(0);
                messages.RemoveAt(0);
            }
                

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
                    case CharacterType.Vampire:
                        newMessage.Content += prompt_vampire;
                        break;
                }
            }

            newMessage.Content += "\nan attitude toward your interlocutor :" + likeGrades[likeGrade] 
                + "\t what you have been doing : " + nowState + prompt_common_gift + "\n"+ prompt_common_info + "\n" + prompt_common_last + "Respond to me : "+inputField.text;

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
            prompt_common_gift = "";
        }
    }
}
