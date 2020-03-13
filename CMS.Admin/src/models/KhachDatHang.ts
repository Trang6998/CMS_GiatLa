import { ChiTietDoGiat } from "@/models/ChiTietDoGiat";
import { NguoiDung } from "@/models/NguoiDung";

export interface KhachDatHang {
    KhachDatHangID: number;
    NguoiDungID: number;
    NgayDat: Date;
    GioDat: number;
    GhiChu: string;
    ThanhTien: number;
    TinhTrangXuLy: number;
    HinhThucThanhToan: number;
    TinhTrangThanhToan: number;
    NguoiDung?: NguoiDung;
    ChiTietDoGiat?: ChiTietDoGiat[];
}
