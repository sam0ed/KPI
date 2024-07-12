import * as SQLite from 'expo-sqlite';
import * as FileSystem from 'expo-file-system';
import { producers, dishes } from './dummyData';

class DatabaseService {
    constructor(dbName) {
        this.dbName = dbName;
        this.database = null;
    }

    async openDatabase() {
        this.database = SQLite.openDatabase(this.dbName);
    }

    async initDB() {
        await this.openDatabase();
        await this.initTables();
        console.log('Database initialized');
    }

    async initTables() {
        const createDishesTable = `CREATE TABLE IF NOT EXISTS Dishes (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name TEXT NOT NULL,
            price INTEGER NOT NULL,
            producer TEXT NOT NULL
        );`;
        const createLogTable = `CREATE TABLE IF NOT EXISTS Log (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            date TEXT NOT NULL,
            dishId INTEGER,
            FOREIGN KEY(dishId) REFERENCES Dishes(id)
        );`
        await this.executeSql(createDishesTable).then(this.executeSql(createLogTable));
        // await this.executeSql(createLogTable);
    }

    async executeSql(sql, params = []) {
        return new Promise((resolve, reject) => {
            this.database.transaction((tx) => {
                tx.executeSql(sql, params,
                    (_, result) => resolve(result),
                    (_, error) => {
                        console.error(`Error executing sql "${sql}" with params "${params}": ${error.message}`);
                        reject(error);
                    }
                );
            });
        });
    }

    async deleteDatabase() {
        try {
            const fullDbPath = `${FileSystem.documentDirectory}SQLite/${this.dbName}`;
            if (this.database) {
                await this.database.closeAsync();
                console.log('Database closed successfully.');
            }
            await FileSystem.deleteAsync(fullDbPath);
            console.log('Database file deleted successfully.');
            this.database = null; // Reset the database reference
        } catch (error) {
            console.error('There was an error closing or deleting the database:', error);
        }
    }

    async addDish(name, price, producer) {
        const insertSql = 'INSERT INTO Dishes (name, price, producer) VALUES (?, ?, ?);';
        return this.executeSql(insertSql, [name, price, producer]);
    }

    async getAllDishes() {
        const selectSql = 'SELECT * FROM Dishes;';
        return this.executeSql(selectSql);
    }

    async addLog(dishId) {
        const date = new Date().toISOString(); // ISO format date
        const insertSql = 'INSERT INTO Log (date, dishId) VALUES (?, ?);';
        return this.executeSql(insertSql, [date, dishId]);
    }
    
    async getAllLogs() {
        const selectSql = 'SELECT * FROM Log;';
        return this.executeSql(selectSql);
    }    

    //////////////////////// methods used for actual work /////////////////////////////

    async populateDB() {
        for (let i = 0; i < dishes.length; i++) {
            await this.addDish(dishes[i].name, dishes[i].price, dishes[i].producer);
        }
        const savedDishes = await this.getAllDishes();

        if (savedDishes.rows._array && savedDishes.rows._array.length > 0) {
            savedDishes.rows._array.forEach(dish => {
                console.log(`Name: ${dish.name}, Price: ${dish.price}, Producer: ${dish.producer}`);
            });
        } else {
            console.log('Population was unsuccessful. No dishes found.');
        }
    }

    async ensureDBReady() {
        const listDbs = await FileSystem.readDirectoryAsync(`${FileSystem.documentDirectory}SQLite`);
        await this.initDB();
        if (!(listDbs && listDbs.includes(this.dbName))) {
            await this.populateDB();
            console.log('Database populated successfully.');
        }
        else {
            console.log('Database already populated.');
        }
    }
}

export default new DatabaseService('dishes.db');

