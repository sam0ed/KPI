import React, { useState, useLayoutEffect, useRef } from "react";
import { GiftedChat } from "react-native-gifted-chat";
import { Button } from "react-native";

const ChatbotApp = ({ navigation }) => {
    const [messages, setMessages] = useState([]);
    const prompt = useRef("");

    useLayoutEffect(() => {
        navigation.setOptions({
            headerRight: () => (
                <Button
                    onPress={clearChat}
                    title="Clear Chat"
                // color="#000" // Optionally customize button color
                />
            ),
        });
    }, [navigation]);

    const TokenEnum = {
        USER: '<|user|>',
        ASSISTANT: '<|assistant|>',
        END: '<|end|>'
    };

    const clearChat = () => {
        setMessages([]);
        prompt.current = "";
    };

    const handleSend = (newMessages = []) => {
        setMessages((previousMessages) =>
            GiftedChat.append(previousMessages, newMessages)
        );

        const userMessage = newMessages[0].text;
        console.log("User message: \n", userMessage);

        tokenizedUserMessages = newMessages.map(message => `${TokenEnum.USER} ${message.text} ${TokenEnum.END}`).join('');
        // console.log("Tokenized user messages: \n", tokenizedUserMessages);
        console.log("Previous prompts: \n", prompt.current);

        getChatbotResponse(prompt.current + tokenizedUserMessages + TokenEnum.ASSISTANT)
            .then(botResponse => {
                // Extracting the 'generated_text' from the first element of the array
                let text_response = botResponse[0].generated_text;
                prompt.current = text_response + TokenEnum.END; 

                console.log("Bot response in text form: \n", text_response);

                const assist_index = text_response.lastIndexOf(TokenEnum.ASSISTANT);
                if (assist_index !== -1) {
                    text_response = text_response.slice(assist_index + TokenEnum.ASSISTANT.length);
                }

                console.log("Bot response cleared: \n", text_response);

                setMessages((previousMessages) =>
                    GiftedChat.append(previousMessages, [
                        {
                            _id: Math.round(Math.random() * 1000000),
                            text: text_response,
                            createdAt: new Date(),
                            user: { _id: 2, name: "Chatbot" },
                        },
                    ])
                );


            });
    };

    const getChatbotResponse = async (prompt) => {
        // const user_token = '<|user|>'
        // const end_token = '<|end|>'
        // const assistant_token = '<|assistant|>'


        try {
            const API_TOKEN = "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";
            const response = await fetch(
                "https://api-inference.huggingface.co/models/microsoft/Phi-3-mini-4k-instruct",
                {
                    headers: { Authorization: `Bearer ${API_TOKEN}`, 'Content-Type': 'application/json' },
                    method: "POST",
                    body: JSON.stringify({inputs: prompt}), // { inputs: `${user_token} ${userMessage} ${end_token} ${assistant_token}` }
                }
            );

            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }

            if (response.headers.get('content-type')?.includes('application/json')) {
                const json_response = await response.json();
                console.log("getChatbotResponse json_response: \n", json_response);

                return json_response;

            } else {
                throw new Error('Received non-JSON response');
            }
        } catch (error) {
            console.error("An error happened", error);
        }

    };


    return (
        <GiftedChat
            messages={messages}
            onSend={(newMessages) => handleSend(newMessages)}
            user={{ _id: 1, name: "User" }}
        />
    );
};

export default ChatbotApp;

