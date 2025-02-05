import { Injectable } from '@angular/core';
import { IAutoMapper, IMapper } from './interfaces';
import { StringHelper } from '../../utilities/contract/string-helper';
import { Mapper } from './mapper';
import { ArrayHelper } from '../../utilities/contract/array-helper';
import { Dictionary } from '../../utilities/dictionary';

interface MappedObject {
  [key: string]: any; // Allow for dynamic properties
}

@Injectable({
  providedIn: 'root'
})

export class AutomapperService implements IAutoMapper {
  private maps: Dictionary<IMapper> = new Dictionary();

  constructor() {} // Removed unused constructor injection

  createMap(from: string, to: string): IMapper {
    StringHelper.throwIsAvailableError(from, "from");
    StringHelper.throwIsAvailableError(to, "to");
    this.throwMissingInDictionaryError(to);

    const key = this.createKey(from, to);
    this.maps.add(key, new Mapper());

    return this.maps.get(key);
  }

  map(from: string, to: string, item: object): any {
    const key = this.createKey(from, to);
    const map = this.maps.get(key);

    if (!map) {
      throw new Error(`The mapping from '${from}' to '${to}' has not been defined.`);
    }

    const mappedProperties = Object
      .keys(map.mappings as { [key: string]: (value: any) => any }) // Use type assertion for clarity
      .reduce(this.assignProperty(map.mappings, item), {});

    // Removed commented-out code (assuming StringToClass is not used)
    const result = { ...mappedProperties }; // Create a new object with mapped properties
    return result;
  }

  mapMany(from: string, to: string, items: any[]): any[] {
    if (!ArrayHelper.isAvailable(items)) {
      return [];
    }

    const results = items.map(item => this.map(from, to, item));
    return results;
  }

  curry(from: string, to: string): (items: any) => any {
    return (item: any) => {
      return this.map(from, to, item);
    };
  }

  curryMany(from: string, to: string): (items: any[]) => any[] {
    return (items: any[]) => {
      return this.mapMany(from, to, items);
    };
  }

  private createKey(from: string, to: string): string {
    const cleanFrom = StringHelper.clean(from);
    const cleanTo = StringHelper.clean(to);
    const delimeter = "[]";
    const key = `${cleanFrom}${delimeter}${cleanTo}`;
    return key;
  }

  private assignProperty(mappings: { [key: string]: (value: any) => any }, item: object): (newObject: MappedObject, key: string) => MappedObject {
    return (newObject, key) => {
      if (mappings.hasOwnProperty(key) && key in newObject) { // Check if key exists in newObject
        newObject[key] = mappings[key](item);
      }
      return newObject;
    };
  }

  private throwMissingInDictionaryError(to: string): void {
    // Removed commented-out code (assuming StringToClass is not used)
  }
}
