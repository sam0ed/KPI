import React, { useState } from 'react';
import { Text, ScrollView, TouchableOpacity, View, StyleSheet } from 'react-native';
import textStyles from '../styles/TextStyles';

const CollapsibleComponent = ({ title, children }) => {
    const [isExpanded, setIsExpanded] = useState(false);

    const handlePress = () => {
        setIsExpanded(!isExpanded);
    };

    return (
        <View stickyHeaderIndices={[0]} style={{ flexDirection: 'column', rowGap: 8, }}>
            <TouchableOpacity onPress={handlePress}>
                <Text style={[textStyles.header1, { marginTop: 16, marginBottom: 8, }]}>{title}</Text>
            </TouchableOpacity>
            {isExpanded && children}
        </View>
    );
};

export default CollapsibleComponent;