export interface LoginResponseDto {
  id:        number;
  fullName:  string;
  email:     string;
  role:      number;
  isActive:  boolean;
  token:     string;
  expiresAt: string;
}

export interface CreateUserDto {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  phone?: string;
}

export interface LoginDto {
  email: string;
  password: string;
}