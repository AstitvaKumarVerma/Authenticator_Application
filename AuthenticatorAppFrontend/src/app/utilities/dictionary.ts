export interface IDictionary<T> {
    get(key: string): T;
    add(key: string, value: T): void;
    has(key: string): boolean;
    put?(key: string, value: T): void; // Optional put method
  }
  
  export class Dictionary<T> implements IDictionary<T> {
    private data: { [key: string]: T } = {}; // Internal object for key-value pairs
  
    get(key: string): T {
      if (!this.has(key)) {
        throw new Error(`The key '${key}' does not exist.`);
      }
      return this.data[key]; // Access value using key directly (type safe)
    }
  
    add(key: string, value: T): void {
      if (this.has(key)) {
        throw new Error(`The key '${key}' already exists. Did you mean to use the method 'put'?`);
      }
      this.data[key] = value; // Add new key-value pair
    }
  
    put(key: string, value: T): void { // Optional method for updating existing values
      if (!this.has(key)) {
        throw new Error(`The key '${key}' does not exist. Use 'add' for new entries.`);
      }
      this.data[key] = value; // Update existing value
    }
  
    has(key: string): boolean {
      return this.data.hasOwnProperty(key); // Check if key exists using hasOwnProperty
    }
  }
  