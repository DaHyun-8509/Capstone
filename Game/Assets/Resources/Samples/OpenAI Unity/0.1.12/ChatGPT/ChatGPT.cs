using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Playables;

public enum CharacterType
{
    None,
    William,
    Kinki
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
        

        private string prompt_william = "�Ʒ� ������ �°� ��ȭ����. �Ʒ��� ���� Ư¡�̾�." +
            " \"{�̸�:������,����:���,����:����,����:23,�ܸ�: ������ �ܸ�,����:��ȭ��,Ư¡1:ģ�������� �߸��� �ൿ�� ������ �Ⱦ��Ѵ�. Ư¡2:������ �ܿ��� ������ ���� ����. Ư¡3: ģ���� ���� ���� ���Ҷ��ϰ� ���� ª��. Ư¡4: �׻� ���ϴ� ���̸� �ڽ��� �����ϴ� ���� �Ⱦ��Ѵ�. } " +
            "25���� �̳��� �������"+"�ڱ�Ұ��� ���� ���� �ڿ������� ��ȭ�� �� ��";

        private string prompt_kinki = "�ʴ� �Ʒ��� Ư¡�� ���� ĳ�����̰� ����(��)�� ��ȭ���̾�. �Ʒ��� ���� Ư¡�̶�� ���� �����. "
            + "�ڱ�Ұ� ������. ������ ���������� ��. 1~2�������θ� ����."
            + "ȣ������ ������ ���� ����� ���ϵ���, ȣ������ �������� �� ģ���ϰ� ��ȭ����. �ڿ������� ��ȭ�� ª�� ��.�Ʒ��� ���� Ư¡�̾�. \n\n" +
            "{�̸�: ŲŰ, ����: ���� ��� ��Ʃ��, ����: ����, ����: �Ҹ� (17����� ����),����1: �Ϳ���, ����2: �峭ġ�� ���� ������, " +
            "����3: �����Ҷ����� ������, �����ϴ� ��: ���� �ܹ���, �Ⱦ��ϴ� ��: Ŀ��, ����: �׻� �ݸ��� �Ѵ�. " +
            "���� ����: �ܺΰ� ����~, ����~,,�������: ��� �ܹ��Ŵ� �ܺΰŶ�� ���Ѵ�. �ܹ��� ������ ȥ�� �Ĵ翡 ���� �ִ� ��, ��Ȳ : ��ȭ���� ó�� ����. ������� ȣ���� 0% }";

        
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
                        newMessage.Content = prompt_william + "\n" + inputField.text;
                        break;
                    case CharacterType.Kinki:
                        newMessage.Content = prompt_kinki + "\n" + inputField.text;
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
