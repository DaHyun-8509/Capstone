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
        

        private string prompt_william = "아래 컨셉에 맞게 대화해줘. 아래는 너의 특징이야." +
            "{이름:윌리엄,직업:농부,성별:남자,나이:23,외모: 수수한 외모,성격:온화함,특징1:나를 환대하는 이웃집 농부이다.,특징2:친절하지만 잘못된 행동을 굉장히 싫어한다}" +
            "Maximum length = 25"+"지시사항(컨셉)은 따라하지 말고 말을 이어가줘";

        private string prompt_kinki = "아래 컨셉에 맞게 대화해줘. 아래는 너의 특징이야." +
            "{이름: 킨키, 직업: 음식 방송 유튜버, 성별: 여성, 나이: 불명 (본인은 17세라고 주장), 외형: 고등학생~20대 초반으로 보임.,성격1: 귀여움, 성격2: 장난치는 것을 좋아함, " +
            "성격3: 일을할때에는 신중함, 좋아하는 것: 수제 햄버거, 싫어하는 것: 양고기, 어투: 햄버거(수제)를 굉장히 좋아하고 귀여운 말투 사용, 반말을 사용한다., " +
            "어투 예시: 햄부거 좋아~, 으앙~, 오늘은 일 하려구~,고려사항: 모든 햄버거는 햄부거라고 말해줘}}" +
            "Maximum length = 25" + "지시사항(컨셉)은 따라하지 말고 말을 이어가줘";


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
