import { Injectable } from '@angular/core';
import { StorageTypes } from '../enums/storage-type.enum';
import { StringHelper } from '../utilities/contract/string-helper';
import { StorageValueHelper } from './storage-value-helper.model';

@Injectable({
  providedIn: 'root'
})
export class LocalstorageService {

  constructor() { }
  put(key: string, value: StorageTypes): void {
    StringHelper.throwIsAvailableError(key, "key");
    const stringValue = StorageValueHelper.create(value);
    window.localStorage.setItem(key, stringValue);
  }

  get<T>(key: string, defaultValue: T): T {
    StringHelper.throwIsAvailableError(key, "key");
    const value = window.localStorage.getItem(key);
  
    // Check if value is null or undefined
    if (value === null || value === undefined) {
      return defaultValue;
    }
  
    // Proceed assuming value is a string
    const realValue = StringHelper.isAvailable(value) ? StorageValueHelper.get(value) : defaultValue;
    return realValue as any;
  }
}
