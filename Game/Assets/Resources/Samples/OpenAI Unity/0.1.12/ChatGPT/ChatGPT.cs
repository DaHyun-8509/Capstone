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
        string prompt_prev = " \n you are the below character, and talking with (�̸� :" + player_name + ") . don't use  \"\" or : ";

        private string prompt_william =
           "\n\n<<�ʰ� ������ �ι��� ����>>{����� �ܿ��� ������ ���� ���Ҷ��� 23�� ��� ������. ���򿡴� ��������縦 �ϰ� �ִ�. �ִ��� ���Ҷ��� ������ ª�� ���Ѵ�. �ڱ� ���� �� ���Ѵ�.} ";

        private string prompt_kinki =
            "\n\n<<you are>>{ Kinki. 20 years old. speak in a cute way. a internet streamer(mukbang). have no interest in others. so never help others. and little stupid. love hamburger. live alone.";


        private string prompt_cheif =
            "\n\n<<you are >>{Robert. 67 years old, a cheif of the town. usually laugh like 'haha'. know well about town. like beer so drinks every day. }"
           + "favorite gifts are steak and egg. worse gifts are cookie, cake and tomato";

        private string prompt_vampire =
            "\n\n<<�ʰ� ������ �ι��� ����>> {�̸��� ���̰� ����� ���� ����. �ڽ��� �����̾��� �����Ѵ�. ������� ���� ���̿��� ������ ���ƴٴѴ�. ����� ����.'ȣȣ'�� �ٿ� ���Ѵ�.";

        string prompt_common_info = "\n\n<<Information about this town>> \r\n" +
            "1. ���� �ֹ� : ŲŰ, �ι�Ʈ, �����̾�(�̸� �Ҹ�), ������, ��, " + player_name + "(��ȭ���)�� �ִ�. " +
            "\r\n2. ŲŰ�� �Թ� ��Ʃ���� �ܹ��Ÿ� �����ϴ� �����̴�. \r\n�ι�Ʈ�� �����̴�. '�����̾�'�� �ڽ��� �����̾��� �����ϴ� �̽��͸��� �����̴�." +
            "\r\n�������� ������ ��η�, ���ϴ� ���� �����ϴ� ���ڴ�.\r\n���� ������ ������ �Ĵ��� �丮���, �����̴�. " +
            "\r\n���� ��ȭ����� ("+player_name + ")�� ������ ���� �������� �ʾҰ� ��縦 �ϴ� û���̴�. " +
            "\r\n3. ���� ��ȭ�����" +player_name + "�� �޸��ų� ��縦 ���ų�, ������ ���� �� �ൿ�� �ϸ� �������� �Ҹ�ȴ�.\r\n�������� �����ϱ� ���ؼ��� �丮�� �ϰų� �Ĵ翡�� �缭 ������ �Ծ�� �Ѵ�. \r\n";

        string prompt_common_last = "\n\nSay only 1~2 Korean sentence. (use within 20 words) " + "Don't explain upper info. Talk like real character, not chatGPT. Don't say you will help me.\n";

        string prompt_common_gift;

        private void Start()
        {
            button.onClick.AddListener(SendReply);
        }

        void Update()
        {
            
            // EnterŰ ����
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
