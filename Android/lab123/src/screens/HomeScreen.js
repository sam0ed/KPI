import React, { useState, useEffect, useContext } from 'react';
import { View, Text, StyleSheet, ScrollView, Button, Alert, TouchableOpacity } from 'react-native';

import CollapsibleComponent from '../components/CollapsibleComponent';
import DishComponent from '../components/DishComponent';
import textStyles from '../styles/TextStyles.jsx';

import { StatusBar } from 'expo-status-bar';
import Checkbox from 'expo-checkbox';
import MultiSlider from '@ptomasroos/react-native-multi-slider';

import DishesContext from '../context/DishesContext.js';
import DatabaseService from '../data/SQLiteService.js'

export default function HomeScreen({ navigation }) {
    const i_priceRange = [0, 200000];
    const [selectedProducers, setSelectedProducers] = useState({});
    const [filteredDishes, setFilteredDishes] = useState([]);
    const [priceRange, setPriceRange] = useState(i_priceRange);
    const [sliderLength, setSliderLength] = useState(0);

    const dishes = useContext(DishesContext)

    const handleCheck = (producer) => {
        setSelectedProducers((prevSelectedProducers) => ({
            ...prevSelectedProducers,
            [producer]: !prevSelectedProducers[producer],
        }));
    };

    const handleSearch = async () => {
        const selected = Object.entries(selectedProducers)
            .filter(([producer, isSelected]) => isSelected)
            .map(([producer]) => producer);

        if (selected.length === 0) {
            Alert.alert('Incomplete Selection', 'Please select at least one producer.');
        } else {
            try {

                const filtered = dishes.filter(dish =>
                    selected.includes(dish.producer) && dish.price >= priceRange[0] && dish.price <= priceRange[1]
                );

                setFilteredDishes(filtered);
            } catch (error) {
                console.error('Failed to fetch dishes:', error);
                Alert.alert('Error', 'Failed to fetch dishes from the database.');
            }
        }
    };

    const handleClear = () => {
        setSelectedProducers({});
        setPriceRange(i_priceRange);
        setFilteredDishes([]);
    }

    const handleOpen = () => {
        const fetchLog = async ()=>{
            const fetchedLogs = await DatabaseService.getAllLogs()
            const logs = fetchedLogs.rows._array
            navigation.navigate("Open", {logs: logs})
        }
        fetchLog()
    }

    useEffect(() => {
        try {
            if (filteredDishes.length !== 0){
                console.log('Use effect to write searched dishes down works here')
                // console.log(filteredDishes )
                for (let i = 0; i<filteredDishes.length; i++) {
                    console.log(`Writing dish ${filteredDishes[i].id} into db`)
                    DatabaseService.addLog(filteredDishes[i].id)
                }
                Alert.alert('Success', 'Action logged successfully');
            }
        } catch (error){
            console.error('Failed write log into database:', error);
            Alert.alert('Error', 'Failed to log user actions');
        }

    }, [filteredDishes])

    return (
        <View style={styles.container}>
            <ScrollView bounces={false} overScrollMode='never'>

                <CollapsibleComponent title="Filters">
                    {[...new Set(dishes.map(dish => dish.producer))].map((producer, index) => (
                        <TouchableOpacity style={{ flexDirection: 'row', alignItems: 'center' }}
                            onPress={() => handleCheck(producer)}
                            key={index}>
                            <Checkbox
                                style={styles.checkbox}
                                value={selectedProducers[producer]}
                                onValueChange={() => handleCheck(producer)}
                                color={'#00CCFF'}
                            />
                            <Text style={textStyles.text}>{producer}</Text>
                        </TouchableOpacity>
                    ))}
                    <View style={[{ justifyContent: 'center', alignItems: 'center' }]}
                        onLayout={event => {
                            const { width } = event.nativeEvent.layout;
                            setSliderLength(width * 0.8);
                        }}>
                        <View>
                            <Text style={textStyles.text}>{priceRange[0]}$ - {priceRange[1]}$</Text>
                        </View>
                        <MultiSlider
                            values={[priceRange[0], priceRange[1]]}
                            sliderLength={sliderLength}
                            onValuesChange={setPriceRange}
                            min={i_priceRange[0]}
                            max={i_priceRange[1]}
                            step={1}
                            allowOverlap={false}
                            snapped
                            minMarkerOverlapDistance={10}
                            selectedStyle={styles.slider}
                            markerStyle={styles.slider}
                        />
                    </View>
                </CollapsibleComponent>

                <CollapsibleComponent title="Dishes">
                    {filteredDishes.length > 0
                        ?
                        (filteredDishes.map((dish, index) => (
                            <DishComponent key={index} dishName={dish.name} price={dish.price} producer={dish.producer} style={{ height: 100 }} />
                        )))
                        :
                        (
                            <Text style={[textStyles.header2, textStyles.notFoundText]}>Dishes not found</Text>
                        )}

                </CollapsibleComponent>
            </ScrollView>
            <Button title="Open" onPress={handleOpen} style={styles.button} />
            <Button title="Search" onPress={handleSearch} style={styles.button} />
            <Button title="Clear" onPress={handleClear} style={styles.button} />
            <StatusBar style="auto" />
        </View>
    );
};

const styles = StyleSheet.create({
    container: {
        padding: 16,
        flex: 1,
        flexDirection: 'column',
        rowGap: 8,
    },
    button: {
        backgroundColor: 'deepskyblue',
        padding: 10,
        borderRadius: 5,
        alignItems: 'center',
        justifyContent: 'center',
        margin: 10,
    },
    checkbox: {
        width: 20,
        height: 20,
        borderWidth: 1,
        borderColor: 'deepskyblue',
        marginRight: 10,
    },
    slider: {
        backgroundColor: 'deepskyblue'
    }
});