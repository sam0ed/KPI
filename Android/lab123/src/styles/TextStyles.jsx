import { StyleSheet } from 'react-native';

const textStyles = StyleSheet.create({
    header1: {
        fontSize: 24, // equivalent to '1.5rem' assuming 1rem = 16px
        fontWeight: 'bold',
        color: '#333',
    },
    header2: {
        fontSize: 18, // equivalent to '1.3rem' assuming 1rem = 16px
        fontWeight: 'bold',
        color: '#333',
    },
    text: {
        fontSize: 16, // equivalent to '1rem' assuming 1rem = 16px
        color: '#333',
    },
    notFoundText: {
        textAlign: 'center',
        marginTop: 50,
        color: 'red'
    },
});

export default textStyles;