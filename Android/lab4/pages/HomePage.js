import { StatusBar } from 'expo-status-bar';
import { SafeAreaView } from 'react-native-safe-area-context';
import React, { useEffect, useState, useRef } from 'react';
import { StyleSheet, FlatList } from 'react-native';
import sampleFiles from '../data/sample';
import FileItem from '../components/FileItem';
import { Video, Audio, ResizeMode } from 'expo-av';


export default function HomePage({ navigation }) {
    const video = useRef(0)
    const audio = useRef(0)
    const [status, setStatus] = React.useState({});


    const handleFilePress = (file) => {

    };

    return (
        <SafeAreaView style={[styles.container]}>
            {/* <FlatList
                data={sampleFiles}
                renderItem={({ item }) => <FileItem
                    image={item.image}
                    title={item.title}
                    artist={item.artist}
                    duration={item.duration}
                    onPress={() => handleFilePress(item)}
                />}
                keyExtractor={item => item.id}
            /> */}

            {/* <Video
                ref={video}
                source={require('../assets/media/cat_suffer.mp4')}
                useNativeControls>

            </Video> */}
            {/* <Video
                // ref={video}
                // style={styles.video}
                source={{
                    uri: 'https://d23dyxeqlo5psv.cloudfront.net/big_buck_bunny.mp4',
                }}
                useNativeControls
                resizeMode={ResizeMode.CONTAIN}
                isLooping
                // onPlaybackStatusUpdate={status => setStatus(() => status)}
            /> */}
        </SafeAreaView>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        // backgroundColor: '#000',
    },

});
