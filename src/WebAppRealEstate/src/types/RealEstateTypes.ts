import { CITY_ENUM, REAL_ESTATE_ENUM, REAL_ESTATE_VALUE_ENUM } from '@/enums';
import { UserPersonalDataProps } from './UserTypes';

export type RealEstateTypesProps = {
    realEstateType?: REAL_ESTATE_ENUM | null;
    advertisementType?: REAL_ESTATE_VALUE_ENUM | null;
    city?: string;
    price?: number;
    quadrature?: number;
};

export type EditAdvertisementType = {
    title: string;
    realEstateType: REAL_ESTATE_ENUM | null;
    advertisementType: REAL_ESTATE_VALUE_ENUM | null;
    quadrature: number;
    price: number;
    realiseDate: Date;
    description: string;
};

export type PublishAdveretismentProps = {
    title: string;
    type: REAL_ESTATE_ENUM | null;
    advertisementType: REAL_ESTATE_VALUE_ENUM | null;
    quadrature: number;
    price: number;
    realiseDate: Date;
    description: string;
};

export type AdveretismentProps = {
    id: number;
    isFavourite: boolean;
    title: string;
    type: REAL_ESTATE_ENUM | null;
    advertisementType: REAL_ESTATE_VALUE_ENUM | null;
    quadrature: number;
    price: number;
    realiseDate: string;
    description: string;
};

export type ValueProps = {
    value: string;
};

export type PublishProps = {
    obj: PublishAdveretismentProps;
    userDetails: UserPersonalDataProps;
    value: ValueProps;
};

export type Favourites = {
    idAdvertisement: number;
    isChecked: boolean;
};

export type Ids = {
    id: number;
    idUser: string;
};

export type FastSearch = {
    cityType?: CITY_ENUM;
    realEstateType?: REAL_ESTATE_ENUM;
} | null;

export type LocationSearch = {
    radius: number,
    position: {lat: number, lng: number}
};

export type UploadImageType = {
    file: FormData;
    idAdv: number;
};

export type EditAdvertisementProps = {
    title: string;
    quadrature: number;
    price: number;
    description: string;
};

export type ImageType = {
    id: number;
    imageUrl: string;
};
