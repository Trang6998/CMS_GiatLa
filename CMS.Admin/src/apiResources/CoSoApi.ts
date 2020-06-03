import { HTTP } from '@/HTTPServices'
import { BaseApi } from './BaseApi'
import { PaginatedResponse,Pagination } from './PaginatedResponse'
import { CoSo } from '@/models/CoSo'
export interface CoSoApiSearchParams extends Pagination {
    khachDatHangID?:number;
    doGiatID?: number;
    hinhThucGiatID?: number;
}
class CoSoApi extends BaseApi {
    search(searchParams: CoSoApiSearchParams): Promise<PaginatedResponse<CoSo>> {

        return new Promise<PaginatedResponse<CoSo>>((resolve: any, reject: any) => {
            HTTP.get('coso', {
                params: searchParams
            }).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    detail(id: number): Promise<CoSo> {
        return new Promise<CoSo>((resolve: any, reject: any) => {
            HTTP.get('coso/' + id).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    update(id: number, coSo: CoSo): Promise<CoSo> {
        return new Promise<CoSo>((resolve: any, reject: any) => {
            HTTP.put('coso/' + id, 
                coSo
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    insert(coSo: CoSo): Promise<CoSo> {
        return new Promise<CoSo>((resolve: any, reject: any) => {
            HTTP.post('coso', 
                coSo
            ).then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
    delete(id: number): Promise<CoSo> {
        return new Promise<CoSo>((resolve: any, reject: any) => {
            HTTP.delete('coso/' + id)
            .then((response) => {
                resolve(response.data);
            }).catch((error) => {
                reject(error);
            })
        });
    }
}
export default new CoSoApi();
