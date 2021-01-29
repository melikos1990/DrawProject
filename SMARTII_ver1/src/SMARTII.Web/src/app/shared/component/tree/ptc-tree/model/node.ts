export class Node<T extends any> {

  constructor(data?: T) {
    this.extend = data;
  }

  id: any;
  name: string;
  children?: Node<T>[] = [];
  hasChildren?: boolean;
  nodeAllowDrag?: boolean;
  nodeAllowDrop?: boolean;
  isDisable?: boolean;
  extend?: T;

}
