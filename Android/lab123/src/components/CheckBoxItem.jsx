// CheckBoxItem.js
import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';

const CheckBoxItem = ({ label, isSelected, onCheck }) => {
    return (
        <TouchableOpacity style={styles.container} onPress={() => onCheck(label)}>
            <View style={[styles.checkbox, isSelected && styles.selected]} />
            <Text style={styles.label}>{label}</Text>
        </TouchableOpacity>
    );
};

const styles = StyleSheet.create({
    container: {
        flexDirection: 'row',
        alignItems: 'center',
        marginBottom: 8,
    },
    checkbox: {
        width: 20,
        height: 20,
        borderWidth: 1,
        borderColor: 'deepskyblue',
        marginRight: 10,
    },
    selected: {
        backgroundColor: 'deepskyblue',
    },
    label: {
        fontSize: 18,
    },
});

export default CheckBoxItem;
