import axios from 'axios'
import { useContext } from 'react'
import { useNavigate } from 'react-router-dom'
import GlobalContext from '../contexts/global'

const host = 'https://localhost:7196/'

export const postLogin = async (username: string, password: string) =>
  await axios.post(host + 'api/Auth/login', {
    username,
    password,
  })

export const getDevices = async (token: string) => {
  return await axios.get(host + 'api/Device', {
    headers: { Authorization: `Bearer ${token}` },
  })
}

export const getDevice = async (token: string, id: number) => {
  return await axios.post(host + 'api/Device/' + id, null, {
    headers: { Authorization: `Bearer ${token}` },
  })
}

export const getDeviceConfigGetByDevice = async (
  token: string,
  deviceId: string,
) => {
  return await axios.get(
    host + 'api/DeviceConfig/byDevice?device_id=' + deviceId,
    {
      headers: { Authorization: `Bearer ${token}` },
    },
  )
}

export const saveDeviceConfigs = async (token: string, obj: any) => {
  return await axios.post(host + 'api/DeviceConfig/save', obj, {
    headers: { Authorization: `Bearer ${token}` },
  })
}

export const deleteDevice = async (token: string, id: number) => {
  return await axios.delete(host + 'api/Device/' + id, {
    headers: { Authorization: `Bearer ${token}` },
  })
}
export const addDevice = async (token: string, obj: any) => {
  return await axios.post(host + 'api/Device', obj, {
    headers: { Authorization: `Bearer ${token}` },
  })
}

export const getRelatorioColeta = async (token: string, id: string | number) => {
  return await axios.get(host + 'api/RelatorioColeta/' + Number(id), {
    headers: { Authorization: `Bearer ${token}` },
  })
}