export type Device = BaseModel & {
  nome: string
  uniqueDeviceId: string
}

export type BaseModel = {
  id: number
  createdOn: Date | string
  updatedDate: Date | string
}

export type DeviceConfig = BaseModel & {
  periodicidade: number
  extras: string
  deviceConfigTypeId: number
}
