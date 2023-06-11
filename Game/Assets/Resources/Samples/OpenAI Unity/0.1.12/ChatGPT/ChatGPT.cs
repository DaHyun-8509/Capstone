using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
            "{�̸�:������,����:���,����:����,����:23,�ܸ�: ������ �ܸ�,����:��ȭ��,Ư¡1:���� ȯ���ϴ� �̿��� ����̴�.,Ư¡2:ģ�������� �߸��� �ൿ�� ������ �Ⱦ��Ѵ�}" +
            "Maximum length = 25"+"���û���(����)�� �������� ���� ���� �̾��";

        private string prompt_kinki = "�Ʒ� ������ �°� ��ȭ����. �Ʒ��� ���� Ư¡�̾�." +
            "{�̸�: ŲŰ, ����: ���� ��� ��Ʃ��, ����: ����, ����: �Ҹ� (������ 17����� ����), ����: ����л�~20�� �ʹ����� ����.,����1: �Ϳ���, ����2: �峭ġ�� ���� ������, " +
            "����3: �����Ҷ����� ������, �����ϴ� ��: ���� �ܹ���, �Ⱦ��ϴ� ��: ����, ����: �ܹ���(����)�� ������ �����ϰ� �Ϳ��� ���� ���, �ݸ��� ����Ѵ�., " +
            "���� ����: �ܺΰ� ����~, ����~, ������ �� �Ϸ���~,�������: ��� �ܹ��Ŵ� �ܺΰŶ�� ������}}" +
            "Maximum length = 25" + "���û���(����)�� �������� ���� ���� �̾��";


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
                Model = "gpt-3.5-turbo-0301",
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
