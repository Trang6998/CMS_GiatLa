<template>
    <v-card style="width: 100%">
        <v-layout row wrap class="ma-3">
            <v-autocomplete v-model="searchParamsThongKeNgay.coSoID"
                            :items="dsCoSo"
                            item-text="TenCoSo"
                            item-value="CoSoID"
                            :readonly="!isAdmin" @change="thayDoiCoSo()"
                            label="Cơ sở / đại lý / CTV thống kê"></v-autocomplete>
        </v-layout>
        <v-layout row wrap class="ma-3">
            
            <h2 style="padding: 10px 10px 0px 0px">Thống kê số liệu trong ngày: </h2>
            <v-flex xs3 style="margin-top: -16px">
                <v-datepicker v-model="searchParamsThongKeNgay.ngay" @input="thongKeNgay()"></v-datepicker>
            </v-flex><v-flex xs5></v-flex>
            <v-flex xs6>
                <v-card style="width: 100%; background-color: #4db6ac" class="pa-3">
                    <h2 style="text-align: center; color: white">Số đơn</h2>
                    <h2 style="text-align: center; color: white">{{thongTinThongKeTrongNgay.soDon}}</h2>
                    <h3 style="text-align: center; color: white"><i>({{thongTinThongKeTrongNgay.soDonTang >= 0 ? 'Tăng ' + thongTinThongKeTrongNgay.soDonTang : 'Giảm ' + Math.abs(thongTinThongKeTrongNgay.soDonTang)}} đơn so với hôm qua)</i></h3>
                </v-card>
            </v-flex>
            <v-flex xs6>
                <v-card style="width: 100%; background-color: #4db6ac" class="pa-3">
                    <h2 style="text-align: center; color: white">Doanh thu</h2>
                    <h2 style="text-align: center; color: white">{{thongTinThongKeTrongNgay.doanhThu}}</h2>
                    <h3 style="text-align: center; color: white"><i>(Tăng {{thongTinThongKeTrongNgay.doanhThuTang}}% so với hôm qua)</i></h3>
                </v-card>
            </v-flex>
        </v-layout>
        <v-layout row wrap class="ma-3">
            <v-flex xs2>
                <v-autocomplete v-model="searchParamsThongKeTuan.tuan"
                                :items="dsTuan" @change="thongKeTuan()"
                                label="Chọn tuần" class="mr-1 ml-1">
                    <template v-slot:selection="data">
                        Tuần {{ data.item }}
                    </template>
                    <template v-slot:item="data">
                        Tuần {{ data.item }}
                    </template>
                </v-autocomplete>
            </v-flex>
            <v-flex xs2>
                <v-autocomplete v-model="searchParamsThongKeTuan.thang"
                                :items="dsThang" @change="thongKeTuan()"
                                label="Chọn tháng" class="mr-1 ml-1">
                    <template v-slot:selection="data">
                        Tháng {{ data.item }}
                    </template>
                    <template v-slot:item="data">
                        Tháng {{ data.item }}
                    </template>
                </v-autocomplete>
            </v-flex>
            <v-flex xs2>
                <v-autocomplete v-model="searchParamsThongKeTuan.nam"
                                :items="dsNam" @change="thongKeTuan()"
                                label="Chọn năm" class="mr-1 ml-1">
                    <template v-slot:selection="data">
                        Năm {{ data.item }}
                    </template>
                    <template v-slot:item="data">
                        Năm {{ data.item }}
                    </template>
                </v-autocomplete>
            </v-flex>
        </v-layout>
        <GChart type="ColumnChart"
                :data="chartData"
                :options="chartOptions" />
    </v-card>
</template>

<script lang="ts">
    import { Component, Vue } from 'vue-property-decorator';
    import ThongKeApi, { ThongKeTuanApiSearchParams , ThongKeNgayApiSearchParams } from '@/apiResources/ThongKeApi';
    import { debug } from 'util';
import { CoSo } from '../models/CoSo';
import CoSoApi, { CoSoApiSearchParams } from '../apiResources/CoSoApi';

    export default Vue.extend({
        data() {
            return {
                chartData: [
                    ['Week', 'Doanh thu (100.000 VND)', 'Số đơn'],
                ],
                chartOptions: {
                    width: 1000,
                    height: 450,
                    chart: {
                        title: 'Company Performance',
                        subtitle: 'Sales, Expenses, and Profit: 2014-2017',
                    }
                },
                dsTuan: [1,2,3,4,5],
                dsThang: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                dsNam: [] as any,
                searchParamsThongKeTuan: {} as ThongKeTuanApiSearchParams,
                searchParamsThongKeNgay: { ngay: this.$moment().format('YYYY/MM/DD') } as ThongKeNgayApiSearchParams,
                thongTinThongKeTrongNgay: {
                    soDon: 0,
                    doanhThu: 0,
                    soDonTang: 0,
                    doanhThuTang: 0
                },
                searchParamsCoSo: { rowsPerPage: 0 } as CoSoApiSearchParams,
                dsCoSo: [] as CoSo[],
                coSoID: this.$store.state.user.Profile.CoSoID,
                isAdmin: this.$store.state.user.Profile.LoaiTaiKhoanID == 4  
            }
        },
        watch: {

        },
        created() {
            this.dsNam = [] as number[]
            var currentYear = this.$moment().year()
            for (let i = -10; i <= 10; i++) {
                this.dsNam.push(parseInt(currentYear) + i);
            }
            this.searchParamsThongKeNgay.coSoID = this.coSoID;
            this.searchParamsThongKeTuan.coSoID = this.coSoID;            
            this.searchParamsThongKeTuan.tuan = this.$moment().week() - this.$moment().startOf('month').week() + 1
            this.searchParamsThongKeTuan.thang = this.$moment().month() + 1
            this.searchParamsThongKeTuan.nam = currentYear;
            this.thongKeTuan()
            this.thongKeNgay()
            this.getCoSo();
        },
        methods: {
            thayDoiCoSo() {
                this.searchParamsThongKeTuan.coSoID = this.searchParamsThongKeNgay.coSoID;

                this.thongKeNgay()
                this.thongKeTuan()
            },
            thongKeTuan() {
                this.chartData = [
                    ['Week', 'Doanh thu (100.000 VND)', 'Số đơn'],
                ]
                ThongKeApi.thongKeTuan(this.searchParamsThongKeTuan).then(res => {
                    for (let i = 0; i < res.length; i++) {
                        var item = [res[i].GiaTri1, res[i].GiaTri2, res[i].GiaTri3];
                        this.chartData.push(item)
                    }
                })
            },
            thongKeNgay() {
                ThongKeApi.thongKeNgay(this.searchParamsThongKeNgay).then(res => {
                    this.thongTinThongKeTrongNgay = res
                })
            },
            getCoSo(): void {
                CoSoApi.search(this.searchParamsCoSo).then(res => {
                    this.dsCoSo = res.Data;
                })
            },
        }
    })
</script>
