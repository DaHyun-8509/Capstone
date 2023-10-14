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

        public static string[] likeGrades = { "ó�� ���� ����", "���� �ֹ�", "�� �ƴ� ���", "ģ��", "��ģ" };
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

        public string nowState = "��ȭ��";

        public static string player_name = "example";

        string prompt_prev = "�Ʒ��� ���û��׿� �°� ��ȭ����. "
            + " \n �ʴ� ���� (�̸� :"+ player_name +") �� ��ȭ�ϰ� �־�. �Ʒ��� ���� ������ ������ ������ �°� ��ȭ����. ȣ������ �������� ģ���ϰ�, �������� �����ϰ� ������.";

        private string prompt_william =
           "\n\n<<��(chatGPT)�� ����>>\n{����� �ܿ��� ������ ���� ���Ҷ��� 23�� ��� ������. ���򿡴� ��������縦 �ϰ� �ִ�. �ִ��� ���Ҷ��� ������ ª�� ���Ѵ�. �ڱ� ���� �� ���Ѵ�.} ";

        private string prompt_kinki =
            "\n\n<<���� ����>>\n{ �ܹ��Ÿ� ���� �����ϴ� �Թ���Ʃ�� ŲŰ. ���̴� 17���̸� �Ϳ��� ������ ���Ѵ�. ���� 6�ø��� ����� �Ѵ�.";

        private string prompt_cheif =
             "\n\n<<���� ����>>\n{������ ������ 67�� �߳� ���� �ι�Ʈ. ȣ���� ������, '������' �ϰ� ���� ���� �������̴�. ������ ���� �� �˰� ������ ������ ��翡 ���� �˷��ְ� �;�������, ģ�������� �ʴ�.";

        string prompt_common_info = "\n\n<<������ ���� ����>> \r\n" +
            "1. ���� �ֹ� : ŲŰ, �ι�Ʈ, �����̾�(�̸� �Ҹ�), ������, ��, " + player_name + "(��ȭ���)�� �ִ�. " +
            "\r\n2. ŲŰ�� �Թ� ��Ʃ���� �ܹ��Ÿ� �����ϴ� �����̴�. \r\n�ι�Ʈ�� �����̴�. '�����̾�'�� �ڽ��� �����̾��� �����ϴ� �̽��͸��� �����̴�." +
            "\r\n�������� ������ ��η�, ���ϴ� ���� �����ϴ� ���ڴ�.\r\n���� ������ ������ �Ĵ��� �丮���, �����̴�. " +
            "\r\n"+player_name + "(��ȭ���)�� ������ ���� �������� �ʾҰ� ��縦 ���� �ִ� û���̴�. " +
            "\r\n3. ��ȭ�����" +player_name + "�� �޸��ų� ��縦 ���ų�, ������ ���� �� �ൿ�� �ϸ� �������� �Ҹ�ȴ�.\r\n�������� �����ϱ� ���ؼ��� �丮�� �ϰų� �Ĵ翡�� �缭 ������ �Ծ�� �Ѵ�. \r\n";

        string prompt_common_last = "\n\n1~2�������θ� ������!ª�� 20�� �̳��� �ܾ�� ������." + "���� ������ ���� �ϰ� ���������� ��. �ڿ������� ��ȭ�� �� ��. �׸��� ȣ������ �������� ���� ģ���� ������ ������. chatGPT���� �ʰ�, ���� Ư¡�� �� ����� ��¥ ĳ����ó�� ����\n\n\n";

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

            newMessage.Content += "\n������� ����:" + likeGrades[likeGrade] + "\t ���� �ʰ� �ϰ��ִ� �� : " +nowState + prompt_common_info + "\n" + prompt_common_last + inputField.text;

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
