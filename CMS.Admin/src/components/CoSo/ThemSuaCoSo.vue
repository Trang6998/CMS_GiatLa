<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập cơ sở' : 'Thêm mới cơ sở' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form ref="form" v-model="valid" scope="formCoSo">
                    <v-layout row wrap>
                        <v-flex xs12>
                            <v-text-field v-model="coSo.TenCoSo"
                                          label="Tên cơ sở / đại lý / CTV"
                                          :counter="50"
                                          :error-messages="errors.collect('Tên cơ sở / đại lý / CTV', 'formCoSo')"
                                          v-validate="'required|max:50'"
                                          data-vv-scope="formCoSo"
                                          data-vv-name="Tên cơ sở / đại lý / CTV">
                            </v-text-field>
                        </v-flex>
                        <v-flex xs12>
                            <v-autocomplete v-model="coSo.LoaiCoSo"
                                            :items="dsLoaiCoSo"
                                            item-text="Ten"
                                            item-value="Value"
                                            label="Phân loại"
                                            :error-messages="errors.collect('Phân loại', 'formDoGiat')"
                                            v-validate="'required'"
                                            data-vv-scope="formDoGiat"
                                            data-vv-name="Phân loại"
                                            required></v-autocomplete>
                        </v-flex>
                        <v-flex xs12>
                            <v-text-field v-model="coSo.DiaChi"
                                          label="Địa chỉ"
                                          :error-messages="errors.collect('Địa chỉ', 'formCoSo')"
                                          v-validate="''"
                                          data-vv-scope="formCoSo"
                                          data-vv-name="Địa chỉ">
                            </v-text-field>
                        </v-flex>
                    </v-layout>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn text @click.native="hide">Hủy</v-btn>
                <v-btn text @click.native="save" color="teal lighten-2"
                       :loading="loadingSave">Lưu</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import CoSoApi, { CoSoApiSearchParams } from '@/apiResources/CoSoApi';
    import { CoSo } from '@/models/CoSo';

    export default Vue.extend({
        data() {
            return {
                isUpdate: false,
                valid: false,
                isShow: false,
                coSo: {} as CoSo,
                loading: false,
                loadingSave: false,
                coSoID: 0,
                searchParamsCoSo: {} as CoSoApiSearchParams,
                dsLoaiCoSo: [
                    {
                        Ten: 'Cơ sở',
                        Value: 1
                    },
                    {
                        Ten: 'Đại lý',
                        Value: 2
                    },
                    {
                        Ten: 'Cộng tác viên',
                        Value: 3
                    }
                ] as any[]
            };
        },
        watch: {
            isShow() {
                this.$validator.errors.clear();
            }
        },
        $_veeValidate: {
            validator: 'new'
        },
        created() {
        },
        methods: {
            hide() {
                this.isShow = false
            },
            show(isUpdate: boolean, item: any) {
                this.isShow = true;
                this.loadingSave = false;
                this.isUpdate = isUpdate;
                if (isUpdate) {
                    this.coSoID = item.CoSoID;
                    this.getDataFromApi(this.coSoID);
                }
                else {
                    this.coSo = {} as CoSo;
                }
            },
            getDataFromApi(id: number): void {
                CoSoApi.detail(id).then(res => {
                    this.coSo = res;
                });
            },
            save(): void {
                this.$validator.validateAll('formCoSo').then((res) => {
                    if (res) {
                        if (this.isUpdate) {
                            this.loading = true;
                            this.loadingSave = true;
                            CoSoApi.update(this.coSoID, this.coSo).then(res => {
                                this.loading = false;
                                this.isShow = false;
                                this.loadingSave = false;
                                this.$emit("save");
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = true;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            this.loadingSave = true;
                            CoSoApi.insert(this.coSo).then(res => {
                                this.coSo = res;
                                this.isUpdate = true;
                                this.loadingSave = false;
                                this.loading = false;
                                this.isShow = false;
                                this.$emit("save");
                                this.$snotify.success('Thêm mới thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.loadingSave = true;
                                this.$snotify.error('Thêm mới thất bại!');
                            });
                        }
                    }
                });
            },
        }
    });
</script>
