import client from "./client";

export interface UpdateUserDto {
  firstName: string;
  lastName:  string;
  phone?:    string;
}

const UserService = {
  update: async (id: number, dto: UpdateUserDto) => {
    const { data } = await client.put(`/Users/${id}`, dto);
    return data;
  },
};

export default UserService;