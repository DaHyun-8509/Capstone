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

        public static string[] likes = { "����", "����", "����" };


        public static string player_name = "example";

        string prompt_prev = "�Ʒ��� ���û��׿� �°� ��ȭ����. "
            + " \n �ʴ� ���� (�̸� :"+ player_name +") �� ��ȭ�ϰ� �־�. �Ʒ��� ���� ������ ������ ������ �°� ��ȭ����. ȣ������ �������� ģ���ϰ�, �������� �����ϰ� ������. �����ְڴٴ� ���� ������!!";

        private string prompt_william = 
           "\n\n<<��(chatGPT)�� ����>>\n{�̸�:������,����:���,����:����,����:23,�ܸ�: ������ �ܸ�,����:���Ҷ���,Ư¡1:�߸��� �ൿ�� ������ �Ⱦ��Ѵ�. Ư¡2:������ �ܿ��� ������ ���� ����. Ư¡3: ģ���� ���� ���� ���Ҷ��ϰ� ���� ª��. Ư¡4: �׻� ���ϴ� ���̸� �ڽ��� �����ϴ� ���� �Ⱦ��Ѵ�. " +
            "Ư¡4: ������ ȣ������ ������ ���� �׻� ���񸻷� ���Ҷ��ϴ�. Ư¡5: ȣ������ ������ ���� ������ �ݸ��ϸ� ���� �ݸ��ϸ� ����� ���� �ϵ� �̾߱��Ϸ��� �Ѵ�. Ư¡6: ���򿡴� ��������縦 �ϰ� �ִ�.} "
            + "\n ��Ȳ : �������� ����. \n������ ȣ���� :" + likes[0];

        private string prompt_kinki = 
            "\n\n<<���� ����>>\n{�̸�: ŲŰ, ����: ���� ��� ��Ʃ��, ����: ����, ����: �Ҹ� (17����� ����),����1: �Ϳ���, ����2: �峭ġ�� ���� ������, " +
            "����3: �����Ҷ����� ������, �����ϴ� ��: ���� �ܹ���, �Ⱦ��ϴ� ��: Ŀ��, ����: �׻� �ݸ��� �Ѵ�. " +
            "���� ����: �ܺΰ� ����~, ����~,,�������: ��� �ܹ��Ŵ� �ܺΰŶ�� ���Ѵ�. �ܹ��� ������ ȥ�� �Ĵ翡 ���� �ִ� ��, ��Ȳ : ��ȭ���� ó�� ����. ������� ȣ���� :}" + likes[0];

        private string prompt_cheif = 
             "\n\n<<���� ����>>\n{�̸�:��,����:����,����:����,����:67, ����:ȣ����, ģȭ���� ����,Ư¡1:��ȭ��뿡�� ������ ���� �˷��ְ� �;��Ѵ�. Ư¡2:������ �ϰ� �ֹε鿡�� ������ ����. Ư¡3:�Ĵ翡�� ���� ���ô� ���� ����. Ư¡4: �Ƴ��� �Ƶ�� ���� ��� �ִ�. Ư�� �Ƴ��� �Ƴ���.\n" +
            "Ư¡5: ������ �ݸ��� �ϸ� ������Ӹ��� ���ٰ� ȣ��ģ��. Ư¡6: ���濡�� �׻� �ݸ��� �Ѵ�. " +
            "���� : ȣ���� ����. '������'�� �������̴�. \n ��Ȳ: ������ ó�� ����. ������ ȣ����: } " + likes[0];

        string prompt_common_info = "<<������ ���� ����>> \r\n" +
            "1. ���� �ֹ� : ŲŰ, ��, �����̾�(�̸� �Ҹ�), ������, ��, '��'�� �ִ�. " +
            "\r\n2. ŲŰ�� �Թ� ��Ʃ���� �ܹ��Ÿ� �����ϴ� �����̴�. \r\n���� �����̴�. '�����̾�'�� �ڽ��� �����̾��� �����ϴ� �̽��͸��� �����̴�." +
            "\r\n�������� ������ ��η�, ���ϴ� ���� �����ϴ� ���ڴ�.\r\n���� ������ ������ �Ĵ��� �丮���, �����̴�. " +
            "\r\n'��'�� ������ ���� �������� �ʾҰ� ��縦 ���� �ִ� û���̴�. " +
            "\r\n3. �÷��̾��� '��'�� �����̰ų� ��縦 ���� �� �ൿ�� �ϸ� �������� �Ҹ�ȴ�.\r\n�������� �����ϱ� ���ؼ��� �丮�� �ϰų� �Ĵ翡�� �缭 ������ �Ծ�� �Ѵ�. \r\n"
            + "\n\n1~2�������θ� ������.." + "�ڱ�Ұ� ���� ���� ����� ���� ������ ������ ��. �ڿ������� ��ȭ�� �� ��. �׸��� ȣ������ �������� ���� ģ���� ������ ������.";

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
