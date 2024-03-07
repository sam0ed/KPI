import React, { useState } from 'react';
import { Text, TouchableOpacity, View, StyleSheet } from 'react-native';

const CollapsibleComponent = ({ title, children }) => {
    const [isExpanded, setIsExpanded] = useState(false);

    const handlePress = () => {
        setIsExpanded(!isExpanded);
    };

    return (
        <View>
            <TouchableOpacity onPress={handlePress}>
                <Text style={styles.title}>{title}</Text>
            </TouchableOpacity>
            {isExpanded && <View>{children}</View>}
        </View>
    );
};

const styles = StyleSheet.create({
    title: {
        fontSize: 24,
        fontWeight: 'bold',
        marginTop: 16,
        marginBottom: 8,
    },
});


export default CollapsibleComponent;