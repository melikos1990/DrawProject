import { Component, OnInit, Injector, Input } from '@angular/core';
import { asAuthTreeNode } from 'src/app/shared/component/tree/function';
import { Node } from '../ptc-tree/model/node';
import { AuthenticationType, PageAuth, Operator } from 'src/app/model/authorize.model';
import { ObjectService } from 'src/app/shared/service/object.service';
import { LayoutBaseComponent } from 'src/app/pages/base/layout-base.component';
import { isNumeric } from 'rxjs/util/isNumeric';

@Component({
  selector: 'app-authentication-tree',
  templateUrl: './authentication-tree.component.html',
  styleUrls: ['./authentication-tree.component.scss']
})
export class AuthenticationTreeComponent extends LayoutBaseComponent implements OnInit {

  @Input() displayDeny = false;
  @Input() disabled: boolean = false;

  @Input() operator: Operator = {
    Feature: new Array<PageAuth>()
  };

  public nodes: Node<AuthenticationType>[] = new Array<Node<AuthenticationType>>();

  constructor(
    public objectSercice: ObjectService,
    public injector: Injector) {
    super(injector);

  }

  ngOnInit() {
    const menuList = this.getTranslateMenu();
    this.nodes = asAuthTreeNode(menuList);
  }

  remove(feature: string) {
    const index = this.operator.Feature.findIndex(x => x.Feature === feature);
    this.operator.Feature.splice(index, 1);
  }

  checkItem($event, feature: string, authticationType: AuthenticationType) {
    const userFeature = this.operator.Feature.find(x => x.Feature === feature);

    if (!userFeature) {
      this.append(feature, authticationType);
      return;
    }

    if ($event.target.checked) {
      userFeature.AuthenticationType += authticationType;
    } else {
      userFeature.AuthenticationType -= authticationType;

    }

  }


  checkAll($event, feature: string) {

    const userFeature = this.operator.Feature.find(x => x.Feature === feature);

    if (!userFeature) {
      this.append(feature, AuthenticationType.All);
    } else {

      userFeature.AuthenticationType = ($event.target.checked) ? AuthenticationType.All : AuthenticationType.None;

    }

  }


  append(feature: string, authenticationType: AuthenticationType) {
    this.operator.Feature.push({
      Feature: feature,
      AuthenticationType: authenticationType
    });
  }

  isComponent = (id) => isNumeric(id) === false;

}
