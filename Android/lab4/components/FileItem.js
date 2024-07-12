// components/FileItem.js
import React from 'react';
import { StyleSheet, Text, TouchableOpacity, View, Image } from 'react-native';

const FileItem = ({ image, title, artist, duration, onPress }) => {
    const imageSource = typeof image === 'string' ? { uri: image } : image;

    return (
        <TouchableOpacity onPress={onPress} style={styles.item}>
            <Image source={ imageSource } style={styles.image} />
            <View style={styles.info}>
                <Text style={styles.title}>{title}</Text>
                <Text style={styles.details}>{artist} • {duration}</Text>
            </View>
        </TouchableOpacity>
    );
};

const styles = StyleSheet.create({
    item: {
        flexDirection: 'row',
        padding: 10,
        // marginVertical: 8,
        marginHorizontal: 16,
        backgroundColor: '#1c1c1c',
        borderRadius: 5,
    },
    image: {
        width: 50,
        height: 50,
        borderRadius: 5,
    },
    info: {
        marginLeft: 10,
        justifyContent: 'center',
    },
    title: {
        fontSize: 16,
        color: '#fff',
    },
    details: {
        fontSize: 14,
        color: '#888',
    },
});

export default FileItem;
