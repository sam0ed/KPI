import React, { useState } from 'react';
import { View, Text, StyleSheet } from 'react-native';
import MultiSlider from '@ptomasroos/react-native-multi-slider';
import textStyles from '../styles/TextStyles';

const PriceRangeSlider = ({ min, max, drilledOnValuesChanged }) => {
    const [values, setValues] = useState([min, max]);
    const [sliderLength, setSliderLength] = useState(0);

    const multiSliderValuesChange = (values) => {
        drilledOnValuesChanged(values);
        setValues(values);
    }

    return (
        <View style={styles.container}
            onLayout={event => {
                const { width } = event.nativeEvent.layout;
                setSliderLength(width*0.8);
            }}>
            <View>
                <Text style={textStyles.text}>{values[0]}$ - {values[1]}$</Text>
            </View>
            <MultiSlider
                values={[values[0], values[1]]}
                sliderLength={sliderLength}
                onValuesChange={multiSliderValuesChange}
                min={min}
                max={max}
                step={1}
                allowOverlap={false}
                snapped
                minMarkerOverlapDistance={10}
                selectedStyle={styles.slider}
                markerStyle={styles.slider}
            />
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        rowGap: 8,

    },
    slider: {
        backgroundColor: 'deepskyblue'
    }
});

export default PriceRangeSlider;

