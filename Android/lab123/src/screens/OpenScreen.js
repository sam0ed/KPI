// import React, { useEffect, useState } from 'react';
// import DatabaseService from '../data/SQLiteService.js'

// // export default function OpenScreen({navigation}) {
// //     return (

// //     );
// // };
// import { FlatList, View, Text } from 'react-native';
// import { ScrollView } from 'react-native-web';
// import textStyles from '../styles/TextStyles.jsx';

// export default function OpenScreen({ route, navigation }) {
//     // const dishes = useContext(DishesContext) 
//     // console.log("\n\n\nLogs in the OpenScreen Look like ")
//     // console.log(route.params.logs)
//     return (
//         <View>
//             <Text style = {[textStyles.header1,  { marginTop: 16, marginLeft: 8, }]}>Logs </Text>
//             {route.params.logs.length > 0 ? (
//                 <FlatList data={route.params.logs} renderItem={(log) => {
//                     log = log.item
//                     let jsx = <Text>Log ID: {log.id}, Timestamp: {log.date}, Dish ID: {log.dishId}</Text>
//                     return jsx
//                 }} >
//                 </FlatList>
//             ) : (
//                 <Text style = {textStyles.notFoundText}>No logs found.</Text>
//             )}
//         </View>
//     );
// };


import React from 'react';
import { ScrollView, View, Text, StyleSheet } from 'react-native';
import textStyles from '../styles/TextStyles.jsx';

export default function OpenScreen({ route, navigation }) {
    const logs = route.params.logs;

    return (
        <View style={styles.container}>
            <Text style={[textStyles.header1, styles.header]}>Logs</Text>
            {logs.length > 0 ? (
                <ScrollView style={styles.table} bounces={false} overScrollMode='never'>
                    <View style={styles.tableHeader}>
                        <Text style={[styles.headerCell, styles.cell]}>Log ID</Text>
                        <Text style={[styles.headerCell, styles.cell]}>Timestamp</Text>
                        <Text style={[styles.headerCell, styles.cell]}>Dish ID</Text>
                    </View>
                    {logs.slice().reverse().map((log, index) => (
                        <View key={index} style={styles.tableRow}>
                            <Text style={styles.cell}>{log.id}</Text>
                            <Text style={styles.cell}>{log.date}</Text>
                            <Text style={styles.cell}>{log.dishId}</Text>
                        </View>
                    ))}
                </ScrollView>
            ) : (
                <Text style={textStyles.notFoundText}>No logs found.</Text>
            )}
        </View>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        padding: 8,
        backgroundColor: '#fff'
    },
    header: {
        marginTop: 16,
        marginLeft: 8
    },
    table: {
        marginTop: 16
    },
    tableHeader: {
        flexDirection: 'row',
        borderBottomWidth: 1,
        borderBottomColor: '#ddd'
    },
    tableRow: {
        flexDirection: 'row',
        paddingVertical: 8,
        borderBottomWidth: 1,
        borderBottomColor: '#ddd'
    },
    cell: {
        flex: 1,
        textAlign: 'center',
        padding: 8
    },
    headerCell: {
        fontWeight: 'bold'
    }
});
