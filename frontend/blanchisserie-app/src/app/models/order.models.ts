export enum OrderStatus {
  Waiting = 0,
  Validated = 1,
  Refused = 2,
}

export interface OrderItem {
  id: number;
  articleName: string;
  price: number;
}

export interface OrderResponse {
  id: number;
  userid: number;
  customerFirstName: string;
  customerLastName: string;
  customerEmail: string;
  orderItems: OrderItem[];
  createdAt: Date;
  status: OrderStatus;
  commentaire: string;
}

export interface OrderRequest {
  userid: number;
  customerFirstName: string;
  customerLastName: string;
  customerEmail: string;
  orderItems: OrderItem[];
  createdAt: Date;
  status: OrderStatus.Waiting;
  commentaire: string;
}
