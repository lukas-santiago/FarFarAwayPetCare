import './ConfiguracaoDispositivoDetalhesRoute.css'

import { useContext, useEffect, useRef, useState } from 'react'
import { Container, Button } from 'react-bootstrap'
import { useNavigate, useParams } from 'react-router-dom'
import GlobalContext from '../../contexts/global'
import {
  deleteDevice,
  getDevice,
  getDeviceConfigGetByDevice,
  saveDeviceConfigs,
} from '../../services/api'
import { Device, DeviceConfig } from '../../types/types'
import { useFormik } from 'formik'
import { ErrorBoundary } from '../../components/ErrorBoundary'

export function ConfiguracaoDispositivoDetalhesRoute() {
  const navigate = useNavigate()
  let { id } = useParams()
  const ctx = useContext(GlobalContext)
  const [device, setDevice] = useState<Partial<Device>>(defaultVar1)
  const [deviceConfig, setDeviceConfig] = useState<DeviceConfig[]>(defaultVar)

  console.log(deviceConfig);
  const formik = useFormik<DeviceConfigForm>({
    initialValues: {
      device: {
        nome: device.nome,
        uniqueDeviceId: device.uniqueDeviceId,
      },
      deviceConfig: {
        ph: {
          ativo: deviceConfig[0]?.periodicidade > 0 || false,
          periodicidade: String(deviceConfig[0]?.periodicidade) || '0',
        },
        temperatura: {
          ativo: deviceConfig[1]?.periodicidade > 0 || false,
          periodicidade: String(deviceConfig[1]?.periodicidade) || '0',
        },
        amonia: {
          ativo: deviceConfig[2]?.periodicidade > 0 || false,
          periodicidade: String(deviceConfig[2]?.periodicidade) || '0',
        },
        imagem: {
          ativo: deviceConfig[3]?.periodicidade > 0 || false,
          periodicidade: String(deviceConfig[3]?.periodicidade) || '0',
        },
        iluminacao: {
          ativo: deviceConfig[4]?.periodicidade > 0 || false,
          inicio: deviceConfig[4]?.extras.split('-')[0] || '00:00',
          fim: deviceConfig[4]?.extras.split('-')[1] || '00:00',
        },
        tomada: {
          ativo: deviceConfig[5]?.periodicidade > 0 || false,
          inicio: deviceConfig[5]?.extras.split('-')[0] || '00:00',
          fim: deviceConfig[5]?.extras.split('-')[1] || '00:00',
        },
        alimentacao: {
          ativo: deviceConfig[6]?.periodicidade > 0 || false,
          inicio: deviceConfig[6]?.extras.split('-')[0] || '00:00',
          fim: deviceConfig[6]?.extras.split('-')[1] || '00:00',
        },
      },
    },
    onSubmit: (values: DeviceConfigForm) => {
      // alert(JSON.stringify(values, null, 2))

      const ph: Partial<DeviceConfig> = {
        deviceConfigTypeId: 1,
        periodicidade: !values.deviceConfig.ph.ativo
          ? 0
          : Number(values.deviceConfig.ph.periodicidade),
        extras: '',
      }
      const temperatura: Partial<DeviceConfig> = {
        deviceConfigTypeId: 2,
        periodicidade: !values.deviceConfig.temperatura.ativo
          ? 0
          : Number(values.deviceConfig.temperatura.periodicidade),
        extras: '',
      }
      const amonia: Partial<DeviceConfig> = {
        deviceConfigTypeId: 3,
        periodicidade: !values.deviceConfig.amonia.ativo
          ? 0
          : Number(values.deviceConfig.amonia.periodicidade),
        extras: '',
      }
      const imagem: Partial<DeviceConfig> = {
        deviceConfigTypeId: 4,
        periodicidade: !values.deviceConfig.imagem.ativo
          ? 0
          : Number(values.deviceConfig.imagem.periodicidade),
        extras: '',
      }
      const iluminacao: Partial<DeviceConfig> = {
        deviceConfigTypeId: 5,
        periodicidade: !values.deviceConfig.iluminacao.ativo ? 0 : 1,
        extras:
          values.deviceConfig.iluminacao.inicio +
          '-' +
          values.deviceConfig.iluminacao.fim,
      }
      const tomada: Partial<DeviceConfig> = {
        deviceConfigTypeId: 6,
        periodicidade: !values.deviceConfig.tomada.ativo ? 0 : 1,
        extras:
          values.deviceConfig.tomada.inicio +
          '-' +
          values.deviceConfig.tomada.fim,
      }
      const alimentacao: Partial<DeviceConfig> = {
        deviceConfigTypeId: 7,
        periodicidade: !values.deviceConfig.alimentacao.ativo ? 0 : 1,
        extras:
          values.deviceConfig.alimentacao.inicio +
          '-' +
          values.deviceConfig.alimentacao.fim,
      }

      const DeviceConfigs = [
        ph,
        temperatura,
        amonia,
        imagem,
        iluminacao,
        tomada,
        alimentacao,
      ]
      console.log(DeviceConfigs)
      saveDeviceConfigs(ctx.user.token, {
        DeviceId: id,
        deviceConfigs: DeviceConfigs,
      })
    },
    enableReinitialize: true,
  })

  useEffect(() => {
    if (JSON.stringify(ctx.user) === JSON.stringify({})) navigate('/login')

    getDevice(ctx.user.token, Number(id)).then((res) => setDevice(res.data))
    getDeviceConfigGetByDevice(ctx.user.token, id!).then((res) =>
      setDeviceConfig(res.data),
    )
  }, [])
  return (
    <Container>
      <h1 className="mb-3">Detalhes do Dispositivo: {device?.nome}</h1>
      <hr />
      <ErrorBoundary>
        <section>
          <form onSubmit={formik.handleSubmit}>
            <div>
              <label>Nome</label>
              <br />
              <input
                type="text"
                name="device.nome"
                onChange={formik.handleChange}
                value={formik.values.device.nome}
              />
            </div>
            <div>
              <label>ID do Dispositivo</label>
              <br />
              <input
                type="text"
                name="device.uniqueDeviceId"
                onChange={formik.handleChange}
                value={formik.values.device.uniqueDeviceId}
              />
            </div>
            <hr />
            <div className="d-flex gap-2 mb-2 align-items-center">
              <div>pH</div>
              <input
                type="checkbox"
                name="deviceConfig.ph.ativo"
                value={formik.values.deviceConfig.ph.ativo}
                checked={formik.values.deviceConfig.ph.ativo}
                onChange={formik.handleChange}
              />
              <select
                name="deviceConfig.ph.periodicidade"
                value={formik.values.deviceConfig.ph.periodicidade}
                onChange={formik.handleChange}
              >
                <option value="0">Selecione um período</option>
                <option value="60">A cada 1 minuto</option>
                <option value="120">A cada 3 minutos</option>
                <option value="300">A cada 5 minutos</option>
                <option value="600">A cada 10 minutos</option>
              </select>
            </div>
            <div className="d-flex gap-2 mb-2 align-items-center">
              <label>Temperatura</label>
              <input
                type="checkbox"
                name="deviceConfig.temperatura.ativo"
                value={formik.values.deviceConfig.temperatura.ativo}
                checked={formik.values.deviceConfig.temperatura.ativo}
                onChange={formik.handleChange}
              />

              <select
                name="deviceConfig.temperatura.periodicidade"
                value={formik.values.deviceConfig.temperatura.periodicidade}
                onChange={formik.handleChange}
              >
                <option value="0">Selecione um período</option>
                <option value="60">A cada 1 minuto</option>
                <option value="120">A cada 3 minutos</option>
                <option value="300">A cada 5 minutos</option>
                <option value="600">A cada 10 minutos</option>
              </select>
            </div>
            <div className="d-flex gap-2 mb-2 align-items-center">
              <label>Amônia</label>
              <input
                type="checkbox"
                name="deviceConfig.amonia.ativo"
                value={formik.values.deviceConfig.amonia.ativo}
                checked={formik.values.deviceConfig.amonia.ativo}
                onChange={formik.handleChange}
              />

              <select
                name="deviceConfig.amonia.periodicidade"
                value={formik.values.deviceConfig.amonia.periodicidade}
                onChange={formik.handleChange}
              >
                <option value="0">Selecione um período</option>
                <option value="60">A cada 1 minuto</option>
                <option value="120">A cada 3 minutos</option>
                <option value="300">A cada 5 minutos</option>
                <option value="600">A cada 10 minutos</option>
              </select>
            </div>
            <div className="d-flex gap-2 mb-2 align-items-center">
              <label>Imagem</label>
              <input
                type="checkbox"
                name="deviceConfig.imagem.ativo"
                value={formik.values.deviceConfig.imagem.ativo}
                checked={formik.values.deviceConfig.imagem.ativo}
                onChange={formik.handleChange}
              />

              <select
                name="deviceConfig.imagem.periodicidade"
                value={formik.values.deviceConfig.imagem.periodicidade}
                onChange={formik.handleChange}
              >
                <option value="0">Selecione um período</option>
                <option value="60">A cada 1 minuto</option>
                <option value="120">A cada 3 minutos</option>
                <option value="300">A cada 5 minutos</option>
                <option value="600">A cada 10 minutos</option>
              </select>
            </div>
            <div className="d-flex gap-2 mb-2 align-items-center">
              <label>Iluminação</label>
              <input
                type="checkbox"
                name="deviceConfig.iluminacao.ativo"
                value={formik.values.deviceConfig.iluminacao.ativo}
                checked={formik.values.deviceConfig.iluminacao.ativo}
                onChange={formik.handleChange}
              />

              <label>Liga às</label>
              <input
                type="time"
                name="deviceConfig.iluminacao.inicio"
                value={formik.values.deviceConfig.iluminacao.inicio}
                onChange={formik.handleChange}
              />
              <label>Desliga às</label>
              <input
                type="time"
                name="deviceConfig.iluminacao.fim"
                value={formik.values.deviceConfig.iluminacao.fim}
                onChange={formik.handleChange}
              />
            </div>
            <div className="d-flex gap-2 mb-2 align-items-center">
              <label>Tomada</label>
              <input
                type="checkbox"
                name="deviceConfig.tomada.ativo"
                value={formik.values.deviceConfig.tomada.ativo}
                checked={formik.values.deviceConfig.tomada.ativo}
                onChange={formik.handleChange}
              />

              <label>Liga às</label>
              <input
                type="time"
                name="deviceConfig.tomada.inicio"
                value={formik.values.deviceConfig.tomada.inicio}
                onChange={formik.handleChange}
              />
              <label>Desliga às</label>
              <input
                type="time"
                name="deviceConfig.tomada.fim"
                value={formik.values.deviceConfig.tomada.fim}
                onChange={formik.handleChange}
              />
            </div>
            <div className="d-flex gap-2 mb-2 align-items-center">
              <label>Alimentação (1 porção)</label>
              <input
                type="checkbox"
                name="deviceConfig.alimentacao.ativo"
                value={formik.values.deviceConfig.alimentacao.ativo}
                checked={formik.values.deviceConfig.alimentacao.ativo}
                onChange={formik.handleChange}
              />

              <label>1ª alimentação às</label>
              <input
                type="time"
                name="deviceConfig.alimentacao.inicio"
                value={formik.values.deviceConfig.alimentacao.inicio}
                onChange={formik.handleChange}
              />
              <label>2ª alimentação às</label>
              <input
                type="time"
                name="deviceConfig.alimentacao.fim"
                value={formik.values.deviceConfig.alimentacao.fim}
                onChange={formik.handleChange}
              />
            </div>
            <div className="d-flex gap-2 mb-2 align-items-center">
              <Button type="submit">Salvar</Button>
              <Button variant="danger" onClick={() => {
                deleteDevice(ctx.user.token, Number(id))
                navigate('/configuracao/dispositivos')
              }}>Deletar</Button>
            </div>
          </form>
        </section>
      </ErrorBoundary>
    </Container>
  )
}

type DeviceConfigForm = {
  device: {
    nome?: string
    uniqueDeviceId?: string
  }
  deviceConfig: {
    ph: {
      ativo: boolean | any
      periodicidade: string
    }
    temperatura: {
      ativo: boolean | any
      periodicidade: string
    }
    amonia: {
      ativo: boolean | any
      periodicidade: string
    }
    imagem: {
      ativo: boolean | any
      periodicidade: string
    }
    iluminacao: {
      ativo: boolean | any
      inicio: string
      fim: string
    }
    tomada: {
      ativo: boolean | any
      inicio: string
      fim: string
    }
    alimentacao: {
      ativo: boolean | any
      inicio: string
      fim: string
    }
  }
}

const defaultVar1 = {
  id: 1,
  nome: 'Aquário 1',
  uniqueDeviceId: 'fd8d872d-3045-47c8-8b24-9db2d4807b7c',
}
const defaultVar = [
  {
    periodicidade: 60,
    extras: '',
    deviceId: 1,
    device: null,
    deviceConfigTypeId: 1,
    deviceConfigType: null,
    deviceData: null,
    id: 2,
    user: null,
    updatedDate: '2022-11-21T16:29:11.3324966',
    createdOn: '2022-11-21T16:29:11.3323775',
  },
  {
    periodicidade: 120,
    extras: '',
    deviceId: 1,
    device: null,
    deviceConfigTypeId: 2,
    deviceConfigType: null,
    deviceData: null,
    id: 3,
    user: null,
    updatedDate: '2022-11-21T16:29:11.4611149',
    createdOn: '2022-11-21T16:29:11.4611132',
  },
  {
    periodicidade: 300,
    extras: '',
    deviceId: 1,
    device: null,
    deviceConfigTypeId: 3,
    deviceConfigType: null,
    deviceData: null,
    id: 4,
    user: null,
    updatedDate: '2022-11-21T16:29:11.4769022',
    createdOn: '2022-11-21T16:29:11.4769007',
  },
  {
    periodicidade: 60,
    extras: '',
    deviceId: 1,
    device: null,
    deviceConfigTypeId: 4,
    deviceConfigType: null,
    deviceData: null,
    id: 5,
    user: null,
    updatedDate: '2022-11-21T16:29:11.4924611',
    createdOn: '2022-11-21T16:29:11.4924595',
  },
  {
    periodicidade: 1,
    extras: '16:28-16:33',
    deviceId: 1,
    device: null,
    deviceConfigTypeId: 5,
    deviceConfigType: null,
    deviceData: null,
    id: 6,
    user: null,
    updatedDate: '2022-11-21T16:29:11.5093579',
    createdOn: '2022-11-21T16:29:11.5093559',
  },
  {
    periodicidade: 1,
    extras: '16:28-18:30',
    deviceId: 1,
    device: null,
    deviceConfigTypeId: 6,
    deviceConfigType: null,
    deviceData: null,
    id: 7,
    user: null,
    updatedDate: '2022-11-21T16:29:11.5235784',
    createdOn: '2022-11-21T16:29:11.5235768',
  },
  {
    periodicidade: 1,
    extras: '08:28-20:28',
    deviceId: 1,
    device: null,
    deviceConfigTypeId: 7,
    deviceConfigType: null,
    deviceData: null,
    id: 8,
    user: null,
    updatedDate: '2022-11-21T16:29:11.5379468',
    createdOn: '2022-11-21T16:29:11.5379453',
  },
]
