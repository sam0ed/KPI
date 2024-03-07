import React, { useState } from 'react';
import { View, Text, StyleSheet, ScrollView, Button, Alert } from 'react-native';
import CheckBoxItem from './src/components/CheckBoxItem';
import CollapsibleComponent from './src/components/CollapsibleComponent';
import PriceRangeSlider from './src/components/RangeSlider';
import { StatusBar } from 'expo-status-bar';


const authors = ['author1', 'author2', 'author3'];

const dishes = [
  { name: 'Dish 1', price: 5000, author: 'author1' },
  { name: 'Dish 2', price: 15000, author: 'author2' },
  { name: 'Dish 3', price: 25000, author: 'author3' },
  { name: 'Dish 4', price: 7500, author: 'author1' },
  { name: 'Dish 5', price: 20000, author: 'author2' },
];

// export default function App() {
// const [selectedAuthors, setSelectedAuthors] = useState({});
// const [filteredDishes, setFilteredDishes] = useState([]);

// const handleCheck = (author) => {
//   setSelectedAuthors((prevSelectedAuthors) => ({
//     ...prevSelectedAuthors,
//     [author]: !prevSelectedAuthors[author],
//   }));
// };

// const handleSubmit = () => {
//   const selected = Object.entries(selectedAuthors)
//     .filter(([author, isSelected]) => isSelected)
//     .map(([author]) => author);

//   if (selected.length === 0) {
//     Alert.alert('Incomplete Selection', 'Please select at least one author.');
//   } else {
//     // Alert.alert('Selected Authors', selected.join(', '));
//     // console.log('Filtered Dishes:', filteredDishes);
//     const filteredDishes = dishes.filter(dish => selected.includes(dish.author));
//     setFilteredDishes(filteredDishes);

//   }

// };

// return (
//   <ScrollView contentContainerStyle={styles.container}>
//     <CollapsibleComponent title="Filters">
//       {authors.map((author) => (
//         <CheckBoxItem
//           key={author}
//           label={author}
//           isSelected={selectedAuthors[author]}
//           onCheck={handleCheck}
//         />
//       ))}
//       <RangeSlider min={0} max={200000} />
//     </CollapsibleComponent>
//     <Button title="OK" onPress={handleSubmit} />
//     <CollapsibleComponent title="Dishes">
//       {filteredDishes.length > 0 && (
//         <View style={styles.dishesContainer}>
//           {filteredDishes.map((dish, index) => (
//             <Text key={index} style={styles.dishText}>
//               {dish.name} - ${dish.price} - {dish.author}
//             </Text>
//           ))}
//         </View>
//       )}
//     </CollapsibleComponent>
//     <StatusBar style="auto" />
//   </ScrollView>
// );
// };

// const styles = StyleSheet.create({
//   container: {
//     padding: 16,
//   },
//   dishesContainer: {
//     marginTop: 16,
//   },
//   dishText: {
//     fontSize: 16,
//     marginBottom: 8,
//   },
//   container: {
//     padding: 16,
//   },
// });

export default function App() {
  const [selectedAuthors, setSelectedAuthors] = useState({});
  const [filteredDishes, setFilteredDishes] = useState([]);
  // Update for price range
  const [priceRange, setPriceRange] = useState([0, 200000]);

  const handleCheck = (author) => {
    setSelectedAuthors((prevSelectedAuthors) => ({
      ...prevSelectedAuthors,
      [author]: !prevSelectedAuthors[author],
    }));
  };

  // Handler to update price range from PriceRangeSlider
  const handlePriceRangeChange = (values) => {
    setPriceRange(values);
  };

  const handleSubmit = () => {
    const selected = Object.entries(selectedAuthors)
      .filter(([author, isSelected]) => isSelected)
      .map(([author]) => author);

    if (selected.length === 0) {
      Alert.alert('Incomplete Selection', 'Please select at least one author.');
    } else {
      const filtered = dishes.filter(dish =>
        selected.includes(dish.author) && dish.price >= priceRange[0] && dish.price <= priceRange[1]
      );

      setFilteredDishes(filtered);
    }
  };

  return (
    <ScrollView contentContainerStyle={styles.container}>
      <CollapsibleComponent title="Filters">
        {authors.map((author) => (
          <CheckBoxItem
            key={author}
            label={author}
            isSelected={selectedAuthors[author]}
            onCheck={handleCheck}
          />
        ))}
        <PriceRangeSlider
          min={0}
          max={200000}
          drilledOnValuesChanged={handlePriceRangeChange} // Pass the handler here
        />
      </CollapsibleComponent>
      <Button title="OK" onPress={handleSubmit} />
      <CollapsibleComponent title="Dishes">
        {filteredDishes.length > 0 && (
          <View style={styles.dishesContainer}>
            {filteredDishes.map((dish, index) => (
              <Text key={index} style={styles.dishText}>
                {dish.name} - ${dish.price} - {dish.author}
              </Text>
            ))}
          </View>
        )}
      </CollapsibleComponent>
      <StatusBar style="auto" />
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    padding: 16,
  },
  dishesContainer: {
    marginTop: 16,
  },
  dishText: {
    fontSize: 16,
    marginBottom: 8,
  },
});