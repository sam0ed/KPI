// components/Player.js
import React, { useEffect, useState } from 'react';
import { Audio } from 'expo-av';
import { StyleSheet, Text, View, Button } from 'react-native';

const Player = ({ file, onStop }) => {
    const [sound, setSound] = useState(null);
    const [isPlaying, setIsPlaying] = useState(false);

    useEffect(() => {
        return sound
            ? () => {
                sound.unloadAsync();
            }
            : undefined;
    }, [sound]);

    const playSound = async () => {
        const { sound } = await Audio.Sound.createAsync(
            require(`../assets/media/Manuel - GAS GAS GAS(EXTENDED MIX).mp3`) // Add your sample file here
        );
        setSound(sound);
        await sound.playAsync();
        setIsPlaying(true);
    };

    const pauseSound = async () => {
        await sound.pauseAsync();
        setIsPlaying(false);
    };

    const stopSound = async () => {
        await sound.stopAsync();
        setIsPlaying(false);
        onStop();
    };

    useEffect(() => {
        playSound();
    }, []);

    return (
        <View style={styles.player}>
            <Text style={styles.title}>{file.name}</Text>
            <View style={styles.controls}>
                <Button title={isPlaying ? 'Pause' : 'Play'} onPress={isPlaying ? pauseSound : playSound} />
                <Button title="Stop" onPress={stopSound} />
            </View>
        </View>
    );
};

const styles = StyleSheet.create({
    player: {
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        backgroundColor: '#fff',
        padding: 10,
        borderTopWidth: 1,
        borderColor: '#ccc',
    },
    title: {
        fontSize: 18,
        textAlign: 'center',
    },
    controls: {
        flexDirection: 'row',
        justifyContent: 'space-around',
        marginTop: 10,
    },
});

export default Player;
