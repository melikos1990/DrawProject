

export class SelectController {
    
    private _source: any[] = [];
    private defaultCompare = (accumulator, currentValue) => accumulator.order - currentValue.order

    get source(){return this._source;}
    set source(val){ this._source = val; }
    
    constructor(source? : any){
        this.source = source;
    }

    init(data, sortCompare: (accumulator, currentValue) => any = this.defaultCompare){ 
        let newData = data.sort(sortCompare);
        this.source = newData;
    }

    move(item: any, moveIdx){
        let old_idx = this.source.findIndex(x => x.id == item.id);

        if(old_idx > moveIdx){
            this.moveToUp(item, old_idx, moveIdx)
        }
        else if(old_idx < moveIdx){
            this.moveToDown(item, old_idx, moveIdx)
        }
    }

    getComplete(){
        let source = [...this.source];
        
        source.forEach((item, idx, array) => {
            item["order"] = idx + 1;
        })

        return source;
    }

    private moveToUp(item: any, old_idx, moveIdx){

        this.remove(old_idx);
        
        this.insert(item, moveIdx);
    }

    private moveToDown(item: any, old_idx, moveIdx){

        this.insert(item, moveIdx);

        this.remove(old_idx);
        
    }

    private remove = (idx: number) => this.source.splice(idx, 1);
    private insert = (item: any, idx) => this.source.splice(idx, 0, item);

}
