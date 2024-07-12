import { NavigationContainer } from '@react-navigation/native';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import { useState, useEffect } from 'react';
import HomePage from './pages/HomePage';
import { Video, Audio, ResizeMode } from 'expo-av';

import * as ScreenOrientation from 'expo-screen-orientation';

import { View, Text, StyleSheet, Button } from 'react-native';
import React from 'react';
import { MediaProvider } from './context/MediaContext';
import * as FileSystem from 'expo-file-system';


const PagesNavigator = createNativeStackNavigator();

async function loadSettings() {
  const settingsUri = `${FileSystem.bundleDirectory}assets/config/settings.json`;

  try {
      const content = await FileSystem.readAsStringAsync(settingsUri);
      const settings = JSON.parse(content);
      console.log("Settings:", settings);
  } catch (error) {
      console.error("Could not load settings:", error);
  }
}

export default function App() {
  useEffect(() => {

  }, [])

  // return (
  //   <MediaProvider>
  //     <NavigationContainer>
  //       <PagesNavigator.Navigator initialRouteName="Home">
  //         <PagesNavigator.Screen
  //           name="Home"
  //           component={HomePage}
  //         />

  //       </PagesNavigator.Navigator>
  //     </NavigationContainer>
  //   </MediaProvider>
  // )
  const settingsUri = `${FileSystem.bundleDirectory}assets/config`


  const video = React.useRef(null);
  const [status, setStatus] = React.useState({});
  const [localAudio, setLocalAudio] = useState(require('./assets/media/gas.mp3'))
  return (
    <View style={styles.container}>
      <Video
        ref={video}
        style={styles.video}
        source={ localAudio }
        // source={{ uri: "http://d23dyxeqlo5psv.cloudfront.net/big_buck_bunny.mp4" }}
        useNativeControls
        resizeMode="contain"
        isLooping
        onPlaybackStatusUpdate={setStatus}
      />
    </View>
  );

}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    // backgroundColor: '#000',
  },
  video: {
    flex: 1,
    alignSelf: "stretch"
  }
});


