

interface String {
  toHostApiUrl(): string;
  toHostUrl(): string;
  toCustomerUrl(params?: any, urlConcatStr?: string, queryConcatStr?: string): string;
  format(): string;
  decoding(): string;
  newLine: string;
}

interface JSON {
  tryParse(str: string): object | string;
}


interface Array<T> {
  toFlatten<R>(selector?: (data: T) => any[]): R[];
}

JSON.tryParse = function (str: string) {
  try {
    return JSON.parse(str);
  } catch (e) {
    return str;
  }
}

String.prototype.newLine = '\r\n';


String.prototype.toHostApiUrl = function () {
  //return `http://10.2.123.110/SMARTII_DEMO/api/${this}`
  // return `http://10.2.124.26/SMARTII/api/${this}`;
  return `${window.apiUrl}/${this}`;
  //return `http://localhost/SMARTII/api/${this}`;
  //return `https://59.120.130.35/SMARTII_AP/api/${this}`;
  // return `https://10.2.6.60/SmartII_AP/api/${this}`;

  // UAT
  // return `https://10.2.2.201/SMARTII_UAT/api/${this}`; // Other
  // return `https://10.2.2.201/SMARTII_OP_UAT/api/${this}`; // OP

  // 正式機
  //return `https://smart.ptcnec.com.tw/SMARTII/api/${this}`; // Other
  //return `https://smart.ptcnec.com.tw/SMARTII_OP/api/${this}`; // OP
};
String.prototype.toHostUrl = function () {
  //return `http://10.2.123.110/SMARTII_DEMO/${this}`
  // return `http://10.2.124.26/SMARTII/${this}`;
  return `${window.apHost}/${this}`;
  //return `http://localhost/SMARTII/${this}`;
  //return `https://59.120.130.35/SMARTII_AP/${this}`;
  // return `https://10.2.6.60/SmartII_AP/${this}`;

  // UAT
  // return `https://10.2.2.201/SMARTII_UAT/api/${this}`; // Other
  // return `https://10.2.2.201/SMARTII_OP_UAT/api/${this}`; // OP

  // 正式機
  //return `https://smart.ptcnec.com.tw/SMARTII/${this}`; // Other
  //return `https://smart.ptcnec.com.tw/SMARTII_OP/${this}`; // OP

};


Array.prototype.toFlatten = function <R>(selector?: (data) => any[]): R {
  return selector ?
    this.reduce((accumulator, currentValue) => [...accumulator, ...selector(currentValue)], []) :
    this.reduce((accumulator, currentValue) => [...accumulator, ...currentValue], []);
}


String.prototype.format = function () {
  let txt = this.toString();
  for (let i = 0; i < arguments.length; i++) {
    const exp = getStringFormatPlaceHolderRegEx(i);
    txt = txt.replace(exp, (arguments[i] == null ? '' : arguments[i]));
  }
  return cleanStringFormatResult(txt);
}
//讓輸入的字串可以包含{}
function getStringFormatPlaceHolderRegEx(placeHolderIndex) {
  return new RegExp('({)?\\{' + placeHolderIndex + '\\}(?!})', 'gm')
}
//當format格式有多餘的position時，就不會將多餘的position輸出
//ex:
// var fullName = 'Hello. My name is {0} {1} {2}.'.format('firstName', 'lastName');
// 輸出的 fullName 為 'firstName lastName', 而不會是 'firstName lastName {2}'
function cleanStringFormatResult(txt) {
  if (txt == null) return "";
  return txt.replace(getStringFormatPlaceHolderRegEx("\\d+"), "");
}


String.prototype.toCustomerUrl = function(params?: any, urlConcatStr?: string, queryConcatStr?: string){

  if(!params) return this;

  urlConcatStr = urlConcatStr == null ? ";" : urlConcatStr;
  queryConcatStr = queryConcatStr == null ? ";" : queryConcatStr;

  let query = "";
  for(let key in params){
    console.log(`key ${key}, value ${params[key]}`);
    query += `${key}=${params[key]}${queryConcatStr}`;
  }

  return `${this}${urlConcatStr}${query}`;
}

String.prototype.decoding = function(){
  try {
   return decodeURIComponent(this);
  } catch (error) {
    throw error;
  }
}

