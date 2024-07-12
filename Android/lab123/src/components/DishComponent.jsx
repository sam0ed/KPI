import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import textStyles from '../styles/TextStyles';

export default function DishComponent({ dishName, price, producer }) {
    return (
        <View style={styles.container}>
            <Text style={[textStyles.header2]}>{dishName}</Text>
            <View>
                <Text style={[textStyles.text]}>${price}</Text>
                <Text style = {[textStyles.text]}>{producer}</Text>
            </View>
        </View>
    );
};


const styles = StyleSheet.create({
    container: {
        backgroundColor: 'deepskyblue', // Light blue color with semi-transparency
        borderRadius: 8, // Slightly rounded corners
        padding: 20, // Padding inside the container
        flexDirection: 'row', // Use flexbox to layout children horizontally
        alignItems: 'center', // Align items vertically in the center
        justifyContent: 'space-between', // Space between the main dish name and the price/producer block
    },
});
