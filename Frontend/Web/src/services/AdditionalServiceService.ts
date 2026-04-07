import client from "./client";

export interface AdditionalServiceDto {
  id: number;
  name: string;
  description?: string;
  price: number;
  isActive: boolean;
}

const AdditionalServiceService = {
  getAll: async (): Promise<AdditionalServiceDto[]> => {
    const { data } = await client.get("/AdditionalServices");
    return data;
  },
};

export default AdditionalServiceService;