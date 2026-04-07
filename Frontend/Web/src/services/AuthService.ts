import client from "./client";
import type { LoginDto, LoginResponseDto, CreateUserDto } from "../models/User";

const AuthService = {
  login: async (dto: LoginDto): Promise<LoginResponseDto> => {
    const { data } = await client.post("/Auth/login", dto);
    return data;
  },

  register: async (dto: CreateUserDto): Promise<LoginResponseDto> => {
    const { data } = await client.post("/Auth/register", {
      ...dto,
      role: 0,
    });
    return data;
  },
};

export default AuthService;