<template>
    <v-flex xs12>
        <v-card>
            <v-card-text>
                <h2>Danh sách cơ sở / đại lý / CTV</h2>
                <v-layout row nowrap>
                    <v-text-field v-model="searchParamsCoSo.keywords" 
                                  @input="getDataFromApi(searchParamsCoSo)"
                                  hide-details
                                  append-icon="search"
                                  label="Tìm kiếm"
                                  placeholder="Tìm kiếm theo tên..."
                                  class="mb-2 ml-1"></v-text-field>
                    <v-spacer></v-spacer>
                    <v-btn @click="showModalThemSua(false, {})" color="teal lighten-2" style="margin-top: 15px" dark small><v-icon small class="px-0">add</v-icon> Thêm Mới</v-btn>
                </v-layout>
                <v-layout row wrap>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsCoSo"
                                      :loading="loadingTable" 
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsCoSo"
                                      :total-items="searchParamsCoSo.totalItems"                                      
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td class="text-xs-center" style="width:15px">{{props.index + 1}}</td>
                                <td class="text-xs-center">{{ props.item.TenCoSo }}</td>
                                <td class="text-xs-center">{{ props.item.LoaiCoSo == 1 ? "Cơ sở" : (props.item.LoaiCoSo == 2? "Đại lý" : (props.item.LoaiCoSo == 3 ? "Cộng tác viên" : "")) }}</td>
                                <td class="text-xs-center">{{ props.item.DiaChi }}</td>
                                <td class="text-xs-center" style="width:80px;">
                                    <v-layout nowrap>
                                        <v-btn icon small class="mx-0" @click="showModalThemSua(true,props.item)">
                                            <v-icon small color="teal">edit</v-icon>
                                        </v-btn>
                                        <v-btn icon small class="mx-0" @click="confirmDelete(props.item)">
                                            <v-icon small color="pink">delete</v-icon>
                                        </v-btn>
                                    </v-layout>
                                </td>
                            </template>
                        </v-data-table>
                    </v-flex>
                </v-layout>
            </v-card-text>
        </v-card>
        <v-dialog v-model="dialogConfirmDelete" max-width="290">
                    <v-card>
                <v-card-title class="headline">Xác nhận xóa</v-card-title>
                <v-card-text class="pt-3">Bạn có chắc chắn muốn xóa?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn @click.native="dialogConfirmDelete=false" flat>Hủy</v-btn>
                    <v-btn color="red darken-1" @click.native="deleteCoSo" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
        <them-sua-co-so ref="themSuaCoSo" @save='getDataFromApi(searchParamsCoSo)'></them-sua-co-so>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import CoSoApi, { CoSoApiSearchParams } from '@/apiResources/CoSoApi';
    import { CoSo } from '@/models/CoSo';
    import ThemSuaCoSo from './ThemSuaCoSo.vue';

    export default Vue.extend({
        components: {
            ThemSuaCoSo
        },
        data() {
            return {
                search: "",
                dsCoSo: [] as CoSo[],
                tableHeader: [
                    { text: 'STT', align: 'center', sortable: false },
                    { text: 'Tên cơ sở', value: 'TenHinhThuc', align: 'center', sortable: true },
                    { text: 'Phân loại', value: 'GhiChu', align: 'center', sortable: false },
                    { text: 'Địa chỉ', value: 'GhiChu', align: 'center', sortable: false },
                    { text: 'Thao tác', value: 'ThaoTac', align: 'center', sortable: false },
                ],
                searchParamsCoSo: { includeEntities: true, rowsPerPage: 0 } as CoSoApiSearchParams,
                loadingTable: false,
                selectedCoSo: {} as CoSo,
                dialogConfirmDelete: false,
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsCoSo);
        },
        methods: {
            showModalThemSua(isUpdate: boolean, item: any) {
                (this.$refs.themSuaCoSo as any).show(isUpdate, item);
            },
            getDataFromApi(searchParamsCoSo: CoSoApiSearchParams): void {
                this.loadingTable = true;
                CoSoApi.search(searchParamsCoSo).then(res => {
                    this.dsCoSo = res.Data;
                    this.searchParamsCoSo.totalItems = res.Pagination.totalItems;
                    this.loadingTable = false;
                });
            },
            confirmDelete(coSo: CoSo): void {
                this.selectedCoSo = coSo;
                this.dialogConfirmDelete = true;
            },
            deleteCoSo(): void {
                CoSoApi.delete(this.selectedCoSo.CoSoID).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsCoSo);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

