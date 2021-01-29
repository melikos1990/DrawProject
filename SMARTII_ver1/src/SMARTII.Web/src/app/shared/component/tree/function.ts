import { AuthenticationType } from 'src/app/model/authorize.model';
import { Menu, KMClassificationNodeViewModel } from 'src/app/model/master.model';
import { Node } from '../../component/tree/ptc-tree/model/node';
import { OrganizationNodeViewModel } from 'src/app/model/organization.model';
import { Guid } from "guid-typescript";


export function asOrganizationTreeNode(node: OrganizationNodeViewModel): Node<OrganizationNodeViewModel> {

  if (!node) {
    return null;
  }

  const root: Node<OrganizationNodeViewModel> = {
    id: node.ID,
    name: `${node.Name} (${node.DefindName || '無'})`,
    nodeAllowDrag: node.IsPresist === false,
    nodeAllowDrop: node.IsPresist === false,
    extend: node,
    children: []
  };


  if (node.Children && node.Children.length > 0) {

    node.Children.forEach(nodeItem => {
      const child = asOrganizationTreeNode(nodeItem);
      if (child) {
        root.children.push(child);
      }
    });
  }

  return root;

}

export function asAuthTreeNode(menu: Menu[]): Node<AuthenticationType>[] {

  if (!menu) {
    return null;
  }

  const result: Node<AuthenticationType>[] = [];

  menu.forEach(menuItem => {

    // 顯示字樣以及首頁不在此限
    if (menuItem.group || menuItem.home) {
      return;
    }

    const node: Node<AuthenticationType> = {
      id: menuItem.component && menuItem.component.toString().replace('Component', ''),
      name: menuItem.title,
      nodeAllowDrag: false,
      extend: 0,
      children: []
    }

    if (menuItem.children && menuItem.children.length > 0) {
      const children = asAuthTreeNode(menuItem.children);
      if (children) {
        children.forEach(child => node.children.push(child));
      }
    }
    result.push(node);
  });

  return result;

}


export function asKMTreeNode(nodes: KMClassificationNodeViewModel[]): Node<KMClassificationNodeViewModel>[] {

  if (!nodes || nodes.length === 0) {
    return null;
  }

  const result: Node<KMClassificationNodeViewModel>[] = [];

  nodes.forEach(nodeItem => {

    const node: Node<KMClassificationNodeViewModel> = {
      id: nodeItem.ClassificationID || Guid.create(),
      name: nodeItem.IsRoot ? nodeItem.NodeName : nodeItem.ClassificationName,
      nodeAllowDrag: nodeItem.IsRoot === false,
      nodeAllowDrop: nodeItem.IsRoot === false,
      extend: nodeItem,
      children: []
    }

    if (nodeItem.Children && nodeItem.Children.length > 0) {
      const children = asKMTreeNode(nodeItem.Children);
      if (children) {
        children.forEach(child => node.children.push(child));
      }
    }
    result.push(node);
  });

  return result;

}

