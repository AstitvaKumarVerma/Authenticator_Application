import { IMapper } from "./interfaces";

// export class Mapper implements IMapper {
//   readonly mappings = {};

//   forMember(property: any, data: any): IMapper {
//     this.mappings[property] = value => {
//       return data(value);
//     };

//     return this;
//   }
// }

interface MappedProperty {
  // Define allowed types for properties (e.g., string, number, etc.)
}

export class Mapper implements IMapper {
  readonly mappings: { [key: string]: (value: any) => any } = {}; // Typed object

  forMember(property: MappedProperty, data: any): IMapper {
    this.mappings[(property as string)] = (value: any) => {
      return data(value);
    };
    return this;
  }
}
