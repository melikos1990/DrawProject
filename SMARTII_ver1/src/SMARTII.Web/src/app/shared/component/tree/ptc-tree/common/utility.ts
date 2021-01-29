import { TreeNode } from 'angular-tree-component';


export function cloneNode(node: TreeNode) {

  let copyNode = node.options.getNodeClone(node);

  // 深層拷貝屬性, PS: children node 除外
  function deepCopy(obj) {
    const _obj = { ...obj };

    Object.keys(obj).forEach(function (objectKey, index) {

      if (objectKey.toLowerCase() === "children") return;

      if (obj[objectKey] && obj[objectKey] instanceof Array) {
        _obj[objectKey] = obj[objectKey].map(x => deepCopy(x));
      }
      else if (obj[objectKey] && obj[objectKey] instanceof Object) {
        _obj[objectKey] = { ...(obj[objectKey]) };
      }

    });

    return _obj;

  }

  copyNode = deepCopy(copyNode);

  if (node.hasChildren) {
    copyNode.children = node.children.map(child => cloneNode(child));
  }


  return copyNode;
}

