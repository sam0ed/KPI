import * as FileSystem from 'expo-file-system';

const listFilesInDir = async (subpath) => {
    try {
        const directory = `${FileSystem.documentDirectory}` + subpath;
        const result = await FileSystem.readDirectoryAsync(directory);
        console.log('Directory contents:', result);
        return result;
    } catch (error) {
        console.error('Error reading directory content', error);
    }
};

export { listFilesInDir };