import React, { useState, useEffect } from 'react';
import {  Alert } from 'react-native';

import DatabaseService from './src/data/SQLiteService.js';
import { NavigationContainer } from '@react-navigation/native';

import HomeScreen from './src/screens/HomeScreen';
import OpenScreen from './src/screens/OpenScreen.js';
import DishesContext from './src/context/DishesContext.js';
import PagesNavigator from './src/navigation/PagesNavigator.js'


export default function App() {

  const [dishes, setDishes] = useState([]);

  useEffect(() => {
    (async () => {
      try {
        // DatabaseService.deleteDatabase();
        await DatabaseService.ensureDBReady();
        const dishesResult = await DatabaseService.getAllDishes();
        setDishes(dishesResult.rows._array);

      } catch (error) {
        console.error('Failed to fetch dishes:', error);
        Alert.alert('Error', 'Failed to fetch dishes from the database.');
      }
    })();
  }, []);

  return (
    <DishesContext.Provider value={dishes}>
      <NavigationContainer>
        <PagesNavigator.Navigator initialRouteName="Home">
          <PagesNavigator.Screen
            name="Home"
            component={HomeScreen}
          />
          <PagesNavigator.Screen
            name="Open"
            component={OpenScreen}
          />
        </PagesNavigator.Navigator>
      </NavigationContainer>
    </DishesContext.Provider>
  );
};
