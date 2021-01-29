import { Injectable } from '@angular/core';
import { isNumeric } from 'rxjs/util/isNumeric';

@Injectable({
  providedIn: 'root'
})
export class ObjectService {

  jsonDeepClone<T>(obj: T): T {
    return JSON.parse(JSON.stringify(obj))
  }

  objDeepClone(obj: any) {
    var clone = {};
    for (var i in obj) {
      if (obj[i] != null && typeof (obj[i]) == "object")
        clone[i] = this.objDeepClone(obj[i]);
      else
        clone[i] = obj[i];
    }
    return clone;
  }

  convertToFormData(
    model: any,
    form: FormData = null,
    namespace = ''
  ): FormData {

    let formData = form || new FormData();
    let formKey

    if (isNumeric(model)) {
      formData.append(namespace, model.toString());
    }

    for (let propertyName in model) {


      if (!model.hasOwnProperty(propertyName) ||
        model[propertyName] == null ||
        model[propertyName] === undefined) {
        continue;
      }

      let formKey;
      if (isNumeric(propertyName)) {
        formKey = namespace;
      } else {
        formKey = namespace ? `${namespace}.${propertyName}` : propertyName
      }

      if (model[propertyName] instanceof Date) {
        formData.append(formKey, model[propertyName].toISOString())
      } else if (model[propertyName] instanceof Array) {
        model[propertyName].forEach((element, index) => {

          if (element instanceof File) {
            const tempFormKey = `${formKey}[]`
            formData.append(tempFormKey, element)
          } else {
            const tempFormKey = `${formKey}[${index}]`
            this.convertToFormData(element, formData, tempFormKey)
          }
        })
      } else if (
        typeof model[propertyName] === 'object' &&
        !(model[propertyName] instanceof File)
      ) {
        this.convertToFormData(model[propertyName], formData, formKey)
      } else if (model[propertyName] instanceof File) {
        formData.append(`${formKey}`, model[propertyName])
      } else {
        formData.append(formKey, model[propertyName])
      }
    }
    return formData
  }


  cloneNode(node) {

    let copyNode = node.options.getNodeClone(node);

    // 深層拷貝屬性, PS: children node 除外
    function deepCopy(obj) {
      const _obj = { ...obj };

      Object.keys(obj).forEach(function (objectKey, index) {

        if (objectKey.toLowerCase() === "children") {
          return;
        }

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
      copyNode.children = node.children.map(child => this.cloneNode(child));
    }


    return copyNode;
  }

  setDateTimeRange(datetime: Date, days: number) {

    let nowdate = datetime.toJSON().slice(0, 10).replace(/-/g, '/');

    datetime.setDate(datetime.getDate() + days);

    let countDate = datetime.toJSON().slice(0, 10).replace(/-/g, '/');

    let daterange: string;

    if (days > 0) {
      daterange = nowdate + " 00:00:00 - " + countDate + " 23:59:59";
    }
    else {
      daterange = countDate + " 00:00:00 - " + nowdate + " 23:59:59";
    }

    return daterange;
  }

}
