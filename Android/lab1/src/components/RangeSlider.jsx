import React, { useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import MultiSlider from '@ptomasroos/react-native-multi-slider';

const PriceRangeSlider = ({ min, max, drilledOnValuesChanged }) => {
    const [values, setValues] = useState([min, max]);

    const multiSliderValuesChange = (values) =>{
        drilledOnValuesChanged(values);
        setValues(values);
    } 

    return (
        <View style={styles.container}>
            <View style={styles.valuesContainer}>
                <Text style={styles.value}>{values[0]}$</Text>
                <Text> - </Text>
                <Text style={styles.value}>{values[1]}$</Text>
            </View>
            <MultiSlider
                values={[values[0], values[1]]}
                sliderLength={280}
                onValuesChange={multiSliderValuesChange}
                min={min}
                max={max}
                step={1}
                allowOverlap={false}
                snapped
                minMarkerOverlapDistance={10}
            />
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        padding: 20,
    },
    valuesContainer: {
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center',
        marginBottom: 10,
    },
    value: {
        fontSize: 20,
    },
    sliderContainer: {
        flexDirection: 'row',
        justifyContent: 'space-between',
    },
    slider: {
        flex: 1,
        marginHorizontal: -16,
    },
});

export default PriceRangeSlider;

