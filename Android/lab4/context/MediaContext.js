import React, { createContext, useState, useEffect } from 'react';
import { Asset } from 'expo-asset';
import * as FileSystem from 'expo-file-system';

export const MediaContext = createContext({});
const directoryUri = 'C:/Users/vikto/Workspace/GitRepos/KPI/Android/lab2/assets/media';

export const MediaProvider = ({ children }) => {
    const [mediaFiles, setMediaFiles] = useState({ audio: [], video: [] });
    // const localAudio = require('C:/Users/vikto/Workspace/GitRepos/KPI/Android/lab2/assets/media/cat_suffer.mp4');
    const localAudio = require('../assets/media/cat_suffer.mp4')
    

    // useEffect(() => {
    //     const loadMediaFiles = async () => {
    //          // Adjust based on your media folder location
    //         const loadedFiles = { audio: [], video: [] };

    //         try {
    //             const result = await FileSystem.readDirectoryAsync(directoryUri);
    //             const files = result.map(file => directoryUri + file);

    //             for (let filePath of files) {
    //                 let fileType = filePath.split('.').pop(); // Get the extension of file

    //                 if (['mp3', 'wav'].includes(fileType)) { // Check for audio files
    //                     const asset = await Asset.fromModule(filePath).downloadAsync();
    //                     loadedFiles.audio.push(asset);
    //                 } else if (['mp4', 'mov'].includes(fileType)) { // Check for video files
    //                     const asset = await Asset.fromModule(filePath).downloadAsync();
    //                     loadedFiles.video.push(asset);
    //                 }
    //             }
    //         } catch (error) {
    //             console.error("Failed to load media files", error);
    //         }

    //         setMediaFiles(loadedFiles);
    //     };

    //     loadMediaFiles();
    //     console.log(mediaFiles)
    // }, []);

    return (
        <MediaContext.Provider value={mediaFiles}>
            {children}
        </MediaContext.Provider>
    );
};
