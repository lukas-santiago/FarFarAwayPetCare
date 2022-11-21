import React, { createContext } from 'react'

const GlobalContext = createContext<{ user: any; changeUser: any }>({
  user: { user: '', token: '' },
  changeUser: (value: any) => {},
})

export default GlobalContext
