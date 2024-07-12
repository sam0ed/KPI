


import React from 'react';
import { SafeAreaView, Text, Button } from 'react-native';
import ChatbotApp from './components/ChatbotApp';
import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';

const PagesNavigator = createNativeStackNavigator();

const App = () => { 
	return (
		// <SafeAreaView style={{ flex: 1 }}>
		// 	<Text
		// 		style={{
		// 			marginLeft: 23,
		// 			fontSize: 20,
		// 			marginTop: 20,
		// 			fontWeight: 'bold',
		// 			color: 'blue',
		// 			backgroundColor: 'yellow',
		// 			marginRight: 30
		// 		}}>
		// 		GoofyChat ChatBot App</Text>

		// 	<ChatbotApp />
		// </SafeAreaView>

		<NavigationContainer>
			<PagesNavigator.Navigator initialRouteName="ChatBot App">
				<PagesNavigator.Screen
					name="ChatBot App"
					component={ChatbotApp}
					// options={({ navigation }) => ({
					// 	title: 'ChatBot App',
					// 	headerRight: () => (
					// 		<Button
					// 			onPress={() => clearChat()}
					// 			title="Clear Chat"
					// 		/>
					// 	),
					// })}
				/>

			</PagesNavigator.Navigator>
		</NavigationContainer>
	);
};

export default App;



	// async function query(data) {
	// 	try {
	// 		const API_TOKEN = "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";
	// 		const response = await fetch(
	// 			"https://api-inference.huggingface.co/models/gpt2",
	// 			{
	// 				headers: { Authorization: `Bearer ${API_TOKEN}`, 'Content-Type': 'application/json' },
	// 				method: "POST",
	// 				body: JSON.stringify(data),
	// 			}
	// 		);

	// 		if (!response.ok) {
	// 			throw new Error(`HTTP error! Status: ${response.status}`);
	// 		}

	// 		if (response.headers.get('content-type')?.includes('application/json')) {
	// 			const result = await response.json();
	// 			console.log(result);
	// 			return result;
	// 		} else {
	// 			throw new Error('Received non-JSON response');
	// 		}
	// 	} catch (error) {
	// 		console.error("An error happened", error);
	// 	}
	// }
	// query({ inputs: "The answer to the universe is" }).then((response) => {
	// 	console.log(JSON.stringify(response));
	// });


	// async function query(data) {
	// 	try {
	// 		const API_TOKEN = "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";
	// 		const response = await fetch(
	// 			"https://api-inference.huggingface.co/models/microsoft/DialoGPT-large",
	// 			{
	// 				headers: { Authorization: `Bearer ${API_TOKEN}`, 'Content-Type': 'application/json' },
	// 				method: "POST",
	// 				body: JSON.stringify(data),
	// 			}
	// 		);

	// 		if (!response.ok) {
	// 			throw new Error(`HTTP error! Status: ${response.status}`);
	// 		}

	// 		if (response.headers.get('content-type')?.includes('application/json')) {
	// 			const result = await response.json();
	// 			console.log(result);
	// 			return result;
	// 		} else {
	// 			throw new Error('Received non-JSON response');
	// 		}
	// 	} catch (error) {
	// 		console.error("An error happened", error);
	// 	}
	// }
	// query(
	// 	{ inputs: { past_user_inputs: ["Which movie is the best ?"], generated_responses: ["It is Die Hard for sure."], text: "Can you explain why ?" } }
	// ).then((response) => {
	// 	console.log(JSON.stringify(response));
	// });

	// async function query(data) {
	// 	const API_TOKEN = "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";
	// 	try {
	// 		const response = await fetch(
	// 			"https://api-inference.huggingface.co/models/microsoft/phi-2",
	// 			{
	// 				headers: { Authorization: `Bearer ${API_TOKEN}`, 'Content-Type': 'application/json' },
	// 				method: "POST",
	// 				body: JSON.stringify(data),
	// 			}
	// 		);

	// 		// First check if response is OK to decide how to proceed
	// 		if (!response.ok) {
	// 			// Read the response body according to its content type
	// 			const contentType = response.headers.get('content-type');
	// 			if (contentType && contentType.includes('application/json')) {
	// 				const errorJson = await response.json();
	// 				console.error(`HTTP error! Status: ${response.status}`, errorJson);
	// 				throw new Error(`HTTP error! Status: ${response.status} Response: ${JSON.stringify(errorJson)}`);
	// 			} else {
	// 				const errorText = await response.text();
	// 				console.error(`HTTP error! Status: ${response.status} Response: ${errorText}`);
	// 				throw new Error(`HTTP error! Status: ${response.status} Response: ${errorText}`);
	// 			}
	// 		}

	// 		// Assuming a successful JSON response
	// 		const result = await response.json();
	// 		console.log(result);
	// 		return result;
	// 	} catch (error) {
	// 		console.error("An error happened", error);
	// 	}
	// }

	// query({"inputs": "How can I make sushi?"})
	// .then(response => {
	// 	console.log("Response:", JSON.stringify(response));
	// }).catch(error => {
	// 	console.error("Final Error:", error);
	// }); 